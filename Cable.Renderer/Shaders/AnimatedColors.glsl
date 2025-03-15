uniform float iTime; // Time for animation
uniform vec2 iResolution; // Resolution of the canvas

vec4 main(vec2 fragCoord) 
{
    vec2 uv = fragCoord/iResolution.xy;
    vec3 col = 0.5 + 0.5*cos(iTime+uv.xyx+vec3(0,2,4));
    return vec4(col, 0.0);
}
