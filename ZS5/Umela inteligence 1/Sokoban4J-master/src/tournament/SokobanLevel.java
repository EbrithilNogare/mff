package tournament;

import java.io.File;

import game.ELevelFormat;
import utils.S4JLReader;
import utils.SokReader;

public class SokobanLevel {
	public static int getLevelCount(File file) {
		if (file == null) {
			throw new RuntimeException("'file' is null");
		}
		if (!file.exists()) {
			throw new RuntimeException("'file' does not exist at: " + file.getAbsolutePath());
		}
		if (!file.isFile()) {
			throw new RuntimeException("'file' is not a file: " + file.getAbsolutePath());
		}
		ELevelFormat format = ELevelFormat.getExpectedLevelFormat(file);
		switch (format) {
		case S4JL: return S4JLReader.getLevelNumber(file);
		case SOK: return SokReader.getLevelNumber(file);
		default:
			throw new RuntimeException("Unexpected file extension: " + file.getAbsolutePath());
		}
	}
}
