uniform float iTime;
uniform vec2 iResolution;
uniform vec2 iOffset;

// Simplex noise function
vec2 hash(vec2 p) 
{
    p = vec2(dot(p, vec2(127.1, 311.7)), dot(p, vec2(269.5, 183.3)));
    return -1.0 + 2.0 * fract(sin(p) * 43758.5453123);
}

float simplexNoise(vec2 p) 
{
    const float K1 = 0.366025404; // (sqrt(3)-1)/2
    const float K2 = 0.211324865; // (3-sqrt(3))/6

    vec2 i = floor(p + (p.x + p.y) * K1);
    vec2 a = p - i + (i.x + i.y) * K2;
    vec2 o = (a.x > a.y) ? vec2(1.0, 0.0) : vec2(0.0, 1.0);
    vec2 b = a - o + K2;
    vec2 c = a - 1.0 + 2.0 * K2;

    vec3 h = max(0.5 - vec3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
    vec3 n = h * h * h * h * vec3(dot(a, hash(i)), dot(b, hash(i + o)), dot(c, hash(i + 1.0)));

    return dot(n, vec3(70.0));
}

// Fractal Brownian Motion (fBm) using multiple octaves of noise
float fbm(vec2 p) 
{
    float sum = 0.0;
    float amp = 0.5;
    float freq = 1.0;

    for (int i = 0; i < 5; i++) 
    {
        sum += amp * simplexNoise(p * freq);
        freq *= 2.0;
        amp *= 0.5;
    }
    return sum;
}

vec4 main(vec2 fragCoord) 
{
    vec2 uv = fragCoord / iResolution.xy;
    uv *= 6.0; // Increase scale

    float noiseValue = fbm(uv + iOffset); // Animated fractal noise
    vec3 color = vec3(noiseValue * 0.5 + 0.5); // Map noise to grayscale

    return vec4(color, 1.0);
}
