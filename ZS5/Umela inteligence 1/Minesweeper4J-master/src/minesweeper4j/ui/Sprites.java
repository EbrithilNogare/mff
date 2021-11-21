package minesweeper4j.ui;

import java.awt.image.BufferedImage;
import java.io.IOException;

import javax.imageio.ImageIO;

public class Sprites {
    BufferedImage hidden, empty, flag, mine;
    BufferedImage[] nums = new BufferedImage[9];

    public Sprites() {
        hidden = get("hidden");
        mine = get("mine");
        flag = get("flag");
        empty = get("0");

        for (int i = 1 ; i <= 8 ; ++i)
            nums[i] = get(Integer.toString(i));
    }

    BufferedImage get(String name) {
        try {
            return ImageIO.read(getClass().getResourceAsStream("/images/" + name + ".png"));
        } catch (IOException e) { throw new Error(e); }
    }
}
