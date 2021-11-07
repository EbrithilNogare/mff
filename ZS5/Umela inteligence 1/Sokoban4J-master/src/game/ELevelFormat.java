package game;

import java.io.File;

public enum ELevelFormat {
	S4JL(".s4jl"),
	SOK(".sok");
	
	private String extension;

	private ELevelFormat(String extension) {
		this.extension = extension;
	}

	public String getExtension() {
		return extension;
	}
	
	/**
	 * Returns {@link ELevetFormat} according to the 'file' extension.
	 * @param file
	 * @return
	 */
	public static ELevelFormat getExpectedLevelFormat(File file) {
		return getForExtension(file.getAbsolutePath());
	}
	
	/**
	 * Returns {@link ELevelFormat} for given file extension.
	 * @param extension
	 * @return
	 */
	public static ELevelFormat getForExtension(String extension) {
		extension = extension.toLowerCase();
		for (ELevelFormat format : ELevelFormat.values()) {
			if (extension.endsWith(format.extension)) return format;
		}
		return null;
	}
	
}
