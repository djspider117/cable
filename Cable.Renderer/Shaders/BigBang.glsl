uniform float iTime;
uniform vec2 iResolution;

vec4 main( vec2 fragCoord )
{
	vec2 r = iResolution.xy;
	vec3 c;
	float l,z=iTime;
	for(int i=0;i<3;i++) 
	{
		vec2 uv,p=fragCoord.xy/r;
		uv=p;
		p-=.5;
		p.x*=r.x/r.y;
		z+=.07;
		l=length(p);
		uv+=p/l*(sin(z)+1.)*abs(sin(l*9.-z-z));
		c[i]=.01/length(mod(uv,1.)-.5);
	}
	return vec4(c/l,1);
}