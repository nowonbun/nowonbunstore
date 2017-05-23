package web.only1.Common;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

public class View {
	public static ViewResult resource(final String templateName) {
		return () -> {
			try {
				URI uri = Thread.currentThread().getContextClassLoader().getResource(templateName).toURI();

				File file = new File(uri);
				if (!file.exists()) {
					throw new FileNotFoundException(file.getName());
				}
				long length = file.length();
				byte[] data = new byte[(int) length];
				try (FileInputStream input = new FileInputStream(file)) {
					input.read(data, 0, data.length);
				}
				return new String(data);
			} catch (URISyntaxException | IOException e) {
				throw new RuntimeException(e);
			}
		};
	}

	public static ViewResult nativeCode(final String html) {
		return () -> {
			return html;
		};
	}
}
