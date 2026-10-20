# Builds the fishing-rod cursor animation from the native 110x52 rod art.
# Rod is rotated with RotSprite (scale2x x3 -> rotate -> sample), line + bobber + splash redrawn per frame.
import math, os, sys, json
import numpy as np
from PIL import Image

SRC, OUT = sys.argv[1], sys.argv[2]
os.makedirs(OUT, exist_ok=True)

src = np.array(Image.open(SRC).convert("RGBA"))
H, W = src.shape[:2]
# snap screenshot noise to a clean palette (cluster colours within a small distance, use the cluster mean)
from collections import Counter
cnt = Counter(tuple(int(v) for v in src[y, x, :3]) for y in range(H) for x in range(W) if src[y, x, 3])
clusters = []
for c, n in cnt.most_common():
    for cl in clusters:
        if sum(abs(a - b) for a, b in zip(c, cl[0])) < 24: cl[1].append((c, n)); break
    else: clusters.append([c, [(c, n)]])
snap = {}
for seed, members in clusters:
    tot = sum(n for _, n in members)
    mean = tuple(int(round(sum(c[i] * n for c, n in members) / tot)) for i in range(3))
    for c, _ in members: snap[c] = mean
for y in range(H):
    for x in range(W):
        if src[y, x, 3]: src[y, x, :3] = snap[tuple(int(v) for v in src[y, x, :3])]
PALETTE = sorted(set(snap.values()))
print("palette", len(PALETTE), "colours")
WHITE = min(PALETTE, key=lambda c: abs(c[0]-237)+abs(c[1]-237)+abs(c[2]-238))
WATER = (186, 222, 240)

# ---------------- split layers ----------------
def is_white(c): return c[3] and abs(int(c[0]) - 237) + abs(int(c[1]) - 237) + abs(int(c[2]) - 238) < 30
TIP = (101.5, 2.0)                      # line leaves the tip guide here
BOB_X0, BOB_Y0 = 99, 33                 # bobber box top-left (native)
rod = src.copy(); bob = np.zeros_like(src); line0 = np.zeros_like(src)
for y in range(H):
    for x in range(W):
        if not src[y, x, 3]: continue
        if y >= BOB_Y0 and x >= BOB_X0:
            bob[y, x] = src[y, x]; rod[y, x] = 0
        elif x >= 103 and is_white(src[y, x]) or (y >= 12 and x >= 97 and is_white(src[y, x])):
            line0[y, x] = src[y, x]; rod[y, x] = 0
# along-rod line: white pixels left on the rod layer -> ordered path (redrawn after rotation)
lw = [(x, y) for y in range(H) for x in range(W) if is_white(rod[y, x])]
for x, y in lw: rod[y, x] = 0
left = set(lw); cur = max(left); PATH = [cur]; left.remove(cur)
while left:
    nb = [q for q in left if max(abs(q[0]-cur[0]), abs(q[1]-cur[1])) == 1]
    if not nb:
        q = min(left, key=lambda q: (q[0]-cur[0])**2 + (q[1]-cur[1])**2)
        if (q[0]-cur[0])**2 + (q[1]-cur[1])**2 > 9: break
        nb = [q]
    cur = min(nb, key=lambda q: abs(q[0]-cur[0]) + abs(q[1]-cur[1])); PATH.append(cur); left.remove(cur)
print("line path", len(PATH), "of", len(lw), "pixels")
by, bx = np.where(bob[:, :, 3] > 0)
BOB = Image.fromarray(bob[by.min():by.max() + 1, bx.min():bx.max() + 1])
BOB_OFF = (bx.min(), by.min())                       # bobber sprite top-left at rest
ATTACH = (100 - bx.min(), 0)                         # where the line ties on, inside the bobber sprite
PIVOT = (25.0, 42.0)                                 # hand on the grip, just above the reel

# ---------------- working canvas ----------------
CW = CH = 300
PX, PY = 150, 150
def C(p): return (p[0] - PIVOT[0] + PX, p[1] - PIVOT[1] + PY)

def scale2x(a):
    p = (a[..., 0].astype(np.uint32) << 24) | (a[..., 1].astype(np.uint32) << 16) | (a[..., 2].astype(np.uint32) << 8) | a[..., 3]
    P = np.pad(p, 1, mode="edge")
    B, D, E, F, Hh = P[:-2, 1:-1], P[1:-1, :-2], P[1:-1, 1:-1], P[1:-1, 2:], P[2:, 1:-1]
    ok = (B != Hh) & (D != F)
    e0 = np.where(ok & (D == B), D, E); e1 = np.where(ok & (B == F), F, E)
    e2 = np.where(ok & (D == Hh), D, E); e3 = np.where(ok & (Hh == F), F, E)
    out = np.empty((p.shape[0] * 2, p.shape[1] * 2), np.uint32)
    out[0::2, 0::2], out[0::2, 1::2], out[1::2, 0::2], out[1::2, 1::2] = e0, e1, e2, e3
    return np.stack([(out >> 24) & 255, (out >> 16) & 255, (out >> 8) & 255, out & 255], -1).astype(np.uint8)

S = 8
rod_big = rod
for _ in range(3): rod_big = scale2x(rod_big)
ROD_BIG = Image.fromarray(rod_big)

def rot_rod(ang):
    big = Image.new("RGBA", (CW * S, CH * S))
    big.paste(ROD_BIG, (int((PX - PIVOT[0]) * S), int((PY - PIVOT[1]) * S)))
    big = big.rotate(ang, Image.NEAREST, center=(PX * S, PY * S))
    a = np.array(big)[S // 2::S, S // 2::S].copy()
    a[a[..., 3] < 128] = 0; a[a[..., 3] > 0, 3] = 255
    return a

def rot_pt(p, ang):
    q = C(p); t = math.radians(ang); dx, dy = q[0] - PX, q[1] - PY
    return (PX + dx * math.cos(t) + dy * math.sin(t), PY - dx * math.sin(t) + dy * math.cos(t))

def draw_path(a, ang):
    pts = [rot_pt((x + 0.5, y + 0.5), ang) for x, y in PATH]
    pts = [(px - 0.5, py - 0.5) for px, py in pts]
    for u, v in zip(pts, pts[1:]): seg(a, u, v, WHITE)

def put(a, x, y, col, over=False):
    x, y = int(round(x)), int(round(y))
    if 0 <= x < CW and 0 <= y < CH and (over or a[y, x, 3] == 0): a[y, x] = (*col, 255)

def seg(a, p0, p1, col):
    x0, y0, x1, y1 = map(lambda v: int(round(v)), (*p0, *p1))
    dx, dy = abs(x1 - x0), -abs(y1 - y0); sx = 1 if x0 < x1 else -1; sy = 1 if y0 < y1 else -1; err = dx + dy
    while True:
        put(a, x0, y0, col)
        if x0 == x1 and y0 == y1: break
        e2 = 2 * err
        if e2 >= dy: err += dy; x0 += sx
        if e2 <= dx: err += dx; y0 += sy

def curve(a, p0, p2, bend):
    # quadratic line; bend pushes the middle sideways (perpendicular), + = droop/right of travel
    mx, my = (p0[0] + p2[0]) / 2, (p0[1] + p2[1]) / 2
    dx, dy = p2[0] - p0[0], p2[1] - p0[1]; L = math.hypot(dx, dy) or 1
    cx, cy = mx - dy / L * bend, my + dx / L * bend
    pts = [((1-t)**2*p0[0] + 2*(1-t)*t*cx + t*t*p2[0], (1-t)**2*p0[1] + 2*(1-t)*t*cy + t*t*p2[1]) for t in np.linspace(0, 1, 24)]
    for u, v in zip(pts, pts[1:]): seg(a, u, v, WHITE)

def splash(a, c, rx, ry, drops):
    n = int(rx * 8)
    for k in range(n):
        t = 2 * math.pi * k / n
        if math.sin(t) < -0.2: continue           # back half hidden behind the bobber
        put(a, c[0] + math.cos(t) * rx, c[1] + math.sin(t) * ry, WATER if k % 3 else WHITE, True)
    for dx, dy in drops: put(a, c[0] + dx, c[1] + dy, WHITE, True)

TIP_REST = C(TIP)
BOB_REST = C(BOB_OFF)
FLOAT = (BOB_REST[0] + 4, BOB_REST[1] + 4)         # centre of the red float = click point

# name, rod angle (+ = back over the shoulder), bobber (mode, dx, dy), line bend, splash, duration(s)
FRAMES = [
    ("Idle_0",  0,  ("orig",),            0, None, 0.40),
    ("Idle_1",  0,  ("orig_dip",),        0, None, 0.40),
    ("Cast_0", 16,  ("tip",   3, 24),     4, None, 0.05),
    ("Cast_1", 38,  ("tip",  -7, 21),     5, None, 0.05),
    ("Cast_2", 56,  ("tip", -17, 13),     4, None, 0.10),
    ("Cast_3", 30,  ("tip", -27,  4),     0, None, 0.04),
    ("Cast_4", -8,  ("tip", -16, -26),   -6, None, 0.04),
    ("Cast_5", -10, ("rest",  6, -58),   -8, None, 0.06),
    ("Cast_6", -6,  ("rest", 10, -12),    3, None, 0.05),
    ("Cast_7", -2,  ("rest",  0,  2),     2, (4, 1, [(-5, -3), (5, -3), (-3, -5), (3, -5)]), 0.06),
    ("Cast_8",  0,  ("rest",  0,  1),     2, (7, 2, [(-8, -5), (8, -5), (-6, -7), (6, -7)]), 0.07),
    ("Cast_9",  0,  ("rest",  0,  0),     0, (9, 2, [(-10, -3), (10, -3)]), 0.08),
]
LAND_FRAME = "Cast_7"

raw = []
for name, ang, bspec, bend, spl, dur in FRAMES:
    a = np.zeros((CH, CW, 4), np.uint8)
    rodf = rot_rod(ang)
    tip = rot_pt(TIP, ang)
    mode = bspec[0]
    if mode in ("orig", "orig_dip"):
        dip = 1 if mode == "orig_dip" else 0
        draw_path(a, 0)
        img0 = Image.fromarray(a); img0.alpha_composite(Image.fromarray(rodf)); a = np.array(img0)
        ox, oy = PX - PIVOT[0], PY - PIVOT[1]
        ys, xs = np.where(line0[..., 3] > 0)
        for y, x in zip(ys, xs): a[int(y + oy), int(x + ox)] = line0[y, x]
        if dip: put(a, BOB_REST[0] + ATTACH[0], BOB_REST[1], WHITE)
        img = Image.fromarray(a); img.alpha_composite(BOB, (int(BOB_REST[0]), int(BOB_REST[1]) + dip))
        raw.append((name, np.array(img), dur)); continue
    if mode == "tip": bpos = (tip[0] + bspec[1] - ATTACH[0], tip[1] + bspec[2])
    else:             bpos = (BOB_REST[0] + bspec[1], BOB_REST[1] + bspec[2])
    if spl: splash(a, (FLOAT[0], FLOAT[1] + 2), spl[0], spl[1], spl[2])
    curve(a, tip, (bpos[0] + ATTACH[0], bpos[1]), bend)
    draw_path(a, ang)
    img = Image.fromarray(a)
    img.alpha_composite(Image.fromarray(rodf))           # rod over the line
    img.alpha_composite(BOB, (int(round(bpos[0])), int(round(bpos[1]))))
    raw.append((name, np.array(img), dur))

# ---------------- crop all frames to one shared box ----------------
alpha = np.zeros((CH, CW), bool)
for _, a, _ in raw: alpha |= a[..., 3] > 0
ys, xs = np.where(alpha)
x0, y0, x1, y1 = xs.min() - 1, ys.min() - 1, xs.max() + 2, ys.max() + 2
FW, FH = int(x1 - x0), int(y1 - y0)
HOT = (int(round(FLOAT[0] - x0)), int(round(FLOAT[1] - y0)))

frames = [(n, Image.fromarray(a).crop((x0, y0, x1, y1)), d) for n, a, d in raw]
os.makedirs(os.path.join(OUT, "Frames"), exist_ok=True)
strip = Image.new("RGBA", (FW * len(frames), FH))
for i, (n, f, d) in enumerate(frames):
    f.save(os.path.join(OUT, "Frames", f"Fishing_Rod_{n}.png"))
    strip.paste(f, (i * FW, 0))
strip.save(os.path.join(OUT, "Fishing_Rod_Sheet.png"))

meta = {"frameW": FW, "frameH": FH, "hotX": HOT[0], "hotY": HOT[1],
        "frames": [{"name": n, "duration": d} for n, _, d in frames], "landFrame": LAND_FRAME}
json.dump(meta, open(os.path.join(OUT, "Fishing_Rod_Sheet.json"), "w"), indent=2)
print(json.dumps(meta))
