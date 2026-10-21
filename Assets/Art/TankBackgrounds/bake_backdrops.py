# Port of Tank.BakeBackdrop (Tank.cs) to Python, bit-for-bit.
# Uses .NET's seeded System.Random algorithm so the hills/plants/sand match the game.
import sys, os, math
import numpy as np
from PIL import Image

f32 = np.float32
INT_MAX = 2147483647

class NetRandom:
    MSEED = 161803398
    def __init__(self, seed):
        sa = [0] * 56
        sub = INT_MAX if seed == -2147483648 else abs(seed)
        mj = self.MSEED - sub
        sa[55] = mj
        mk = 1
        for i in range(1, 55):
            ii = (21 * i) % 55
            sa[ii] = mk
            mk = mj - mk
            if mk < 0: mk += INT_MAX
            mj = sa[ii]
        for _ in range(4):
            for i in range(1, 56):
                sa[i] -= sa[1 + (i + 30) % 55]
                if sa[i] < 0: sa[i] += INT_MAX
        self.sa, self.inext, self.inextp = sa, 0, 21
    def _internal(self):
        a = self.inext + 1
        if a >= 56: a = 1
        b = self.inextp + 1
        if b >= 56: b = 1
        r = self.sa[a] - self.sa[b]
        if r == INT_MAX: r -= 1
        if r < 0: r += INT_MAX
        self.sa[a] = r
        self.inext, self.inextp = a, b
        return r
    def sample(self): return self._internal() * (1.0 / INT_MAX)
    def next(self, a, b=None):
        if b is None: return int(self.sample() * a)
        return int(self.sample() * (b - a)) + a
    def next_double(self): return self.sample()

def mix(a, b, t):
    t = f32(t)
    return tuple(int(f32(f32(x) + (f32(y) - f32(x)) * t)) for x, y in zip(a, b))

LEFT, TOP, RIGHT, BOTTOM, SURFACE, FLOOR = 6, 6, 524, 354, 30, 322
TANKS = [  # (habitat index, file name, Top, Bottom, Sand, SandDark)
    (0, "Fresh_Water", (70, 165, 150), (18, 64, 74), (196, 172, 112), (150, 128, 80)),
    (1, "Salt_Water", (80, 160, 230), (14, 44, 110), (232, 214, 166), (190, 168, 120)),
    (2, "Giant_Tank", (46, 96, 170), (6, 16, 44), (150, 142, 124), (104, 98, 86)),
    (3, "Candy_Tank", (255, 175, 214), (160, 84, 168), (255, 232, 242), (236, 180, 210)),
]
CANDY = 3
BAYER = [[0, 8, 2, 10], [12, 4, 14, 6], [3, 11, 1, 9], [15, 7, 13, 5]]

def bake(hab, top_c, bot_c, sand, sand_dark):
    w, h = RIGHT - LEFT, BOTTOM - TOP
    px = np.zeros((h, w, 3), np.uint8)
    def put(x, y, c):
        if 0 <= x < w and 0 <= y < h: px[y, x] = c
    surf, floor = SURFACE - TOP, FLOOR - TOP
    rnd = NetRandom(hab * 97 + 13)
    steps = 7

    grad = [mix(top_c, bot_c, min(1.0, max(0.0, f32(b) / f32(steps)))) for b in range(steps + 2)]
    for y in range(h):
        for x in range(w):
            if y < surf: px[y, x] = (26, 34, 48); continue
            t = f32(f32(y - surf) / f32(floor - surf)) * f32(steps)
            band = int(t)
            if f32(t - f32(band)) * f32(16) > BAYER[y & 3][x & 3]: band += 1
            px[y, x] = grad[band]

    far = mix(bot_c, (0, 0, 0), 0.25)
    for _ in range(7):
        cx = rnd.next(0, w); rx = rnd.next(30, 70); ry = rnd.next(14, 34)
        cy = floor + 4
        dx, dy = f32(rx * rx + f32(0.01)), f32(ry * ry + f32(0.01))
        for yy in range(-ry, ry + 1):
            for xx in range(-rx, rx + 1):
                if f32(f32(xx * xx) / dx) + f32(f32(yy * yy) / dy) <= 1.0: put(cx + xx, cy + yy, far)

    far_plant = mix(bot_c, (20, 60, 40), 0.35)
    if hab == CANDY: far_plant = mix(bot_c, (255, 120, 180), 0.3)
    for _ in range(14):
        bx = rnd.next(0, w); ph = rnd.next(30, 110)
        sd = rnd.next_double() * 10
        for y in range(ph):
            ox = int(round(math.sin(y * 0.08 + sd) * 3))
            for i in range(bx + ox, bx + ox + 3): put(i, floor - y, far_plant)

    light = mix(sand, (255, 255, 255), 0.4)
    crest = mix(sand, (255, 255, 255), 0.3)
    sprinkles = [(255, 90, 140), (90, 200, 255), (255, 220, 80), (140, 230, 120)]
    for x in range(w):
        top = floor + int(round(math.sin(x * 0.04) * 2 + math.sin(x * 0.11) * 1))
        for y in range(top, h):
            c = sand
            r = rnd.next(100)
            if r < 10: c = sand_dark
            elif r < 13: c = light
            if hab == CANDY and r == 50: c = sprinkles[rnd.next(4)]
            if y == top: c = crest
            put(x, y, c)
    return Image.fromarray(px, "RGB")

out = sys.argv[1]
os.makedirs(out, exist_ok=True)
for hab, name, *cols in TANKS:
    img = bake(hab, *cols)
    img.save(os.path.join(out, f"{name}_Backdrop.png"))
    img.resize((img.width * 4, img.height * 4), Image.NEAREST).save(os.path.join(out, f"{name}_Backdrop@4x.png"))
    print(name, img.size)
