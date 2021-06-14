shader_type canvas_item;
render_mode blend_mix;

// uniform float outLineSize  = 0.05;
uniform vec4  outLineColor = vec4(1.0, 1.0, 1.0, 1.0);
uniform bool outlined = true;

void fragment()
{
    vec4 tcol = texture(TEXTURE, UV);

    if (outlined) {
        // Sample colors from the surrounding pixels
        int thickness = 4;
        int size = thickness * 2 + 1;
        int sqsize = size * size;
        float edgeAlphas[441]; // Max size of 10
        for (int i = 0; i < sqsize; i++) {
            int x = i % size - size;
            int y = i / size - size;
            vec2 coord = vec2(SCREEN_PIXEL_SIZE.x * float(x), SCREEN_PIXEL_SIZE.y * float(y));
            edgeAlphas[i] = texture(TEXTURE, UV + coord).a;
        }

        int csize = 3;
        int csqsize = csize * csize;
        vec4 samples[441]; // Max csize of 10
        for (int i = 0; i < sqsize; i++) {
            int x = i % size - size;
            int y = i / size - size;
            vec2 coord = vec2(SCREEN_PIXEL_SIZE.x * float(x), SCREEN_PIXEL_SIZE.y * float(y));
            samples[i] = texture(TEXTURE, UV + coord);
        }

        // Average all the alpha channels
        float avgAlpha = 0.0;
        for (int i = 0; i < sqsize; i++) {
            avgAlpha += edgeAlphas[i];
        }
        avgAlpha = avgAlpha / float(sqsize);

        vec4 avg = vec4(0.0);
        for (int i = 0; i < csqsize; i++) {
            avg += samples[i];
        }
        avg /= float(csqsize);

        // Only apply effects to the edges
        if (avgAlpha > 0.0 && avgAlpha < 1.0) {
            tcol = outLineColor;

            // Average the colors
            tcol = (outLineColor + avg) / 2.0;
            tcol.a = outLineColor.a;

            // Smooth the edges
            if (avgAlpha < 0.1) {
                tcol.a = outLineColor.a * avgAlpha;
            }
        }
    }

    COLOR = tcol;
}
