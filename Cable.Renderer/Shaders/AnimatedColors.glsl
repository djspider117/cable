/* GENERATED WITH SHADERBUILDER */

uniform float iTime;
uniform vec2 iResolution;

vec4 main(vec2 fragCoord){

vec2 uv = fragCoord/iResolution.xy;
vec3 Sa04 = vec3(0, 2, 4);
vec3 D5Cw3gs = iTime + uv.xyx + Sa04;
vec3 K46 = 0.5 * cos(D5Cw3gs);
vec3 B5241 = 0.5 + K46;
return vec4(B5241, 0);

}
