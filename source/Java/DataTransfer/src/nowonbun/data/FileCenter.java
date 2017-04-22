package nowonbun.data;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.util.Properties;

public class FileCenter {
	private static FileCenter instance = null;

	public static FileCenter getInstance() {
		if (instance == null) {
			instance = new FileCenter();
		}
		return instance;
	}

	private String storepath;
	private String backuppath;
	private Properties properties;

	private FileCenter() {
		try (InputStream stream = Thread.currentThread().getContextClassLoader()
				.getResourceAsStream("project.properties")) {
			properties = new Properties();
			properties.load(stream);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
		storepath = properties.getProperty("filestore");
		backuppath = properties.getProperty("filebackup");
		File file = new File(storepath);
		if (!file.exists()) {
			file.mkdirs();
		}
		file = new File(backuppath);
		if (!file.exists()) {
			file.mkdirs();
		}
	}

	public void crateFile(String filename, String data) {
		File file = new File(storepath + File.separatorChar + filename);
		if (file.exists()) {
			file.delete();
		}
		try (FileOutputStream stream = new FileOutputStream(file)) {
			stream.write(data.getBytes("UTF-8"));
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public String readFile(String filename) {
		File file = new File(storepath + File.separatorChar + filename);
		if (!file.exists()) {
			throw new RuntimeException(new FileNotFoundException());
		}
		try (FileInputStream stream = new FileInputStream(file)) {
			byte[] data = new byte[(int) file.length()];
			stream.read(data, 0, data.length);
			return new String(data, "UTF-8");
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public void moveFile(String filename) {
		File file = new File(storepath + File.separatorChar + filename);
		file.renameTo(new File(backuppath + File.separatorChar + filename));
	}

}
