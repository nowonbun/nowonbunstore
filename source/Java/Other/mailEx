package common;

import java.util.Properties;
import javax.mail.Authenticator;
import javax.mail.Message;
import javax.mail.PasswordAuthentication;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;

public class MailSender {

	private static MailSender singleton = null;

	private MailSender() {

	}

	public static void Send(String address, String title, String content) {
		if (singleton == null) {
			singleton = new MailSender();
		}
		singleton.inlineSend(address, title, content);
	}

	public void inlineSend(String address, String title, String content) {

		Properties props = new Properties();
		props.put("mail.smtp.host", PropertyMap.getInstance().getProperty("config", "mail_host"));
		props.put("mail.smtp.auth", "true");
		props.put("mail.smtp.port", "465");
		props.put("mail.smtp.ssl.enable", "true");
		props.put("mail.smtp.ssl.trust", PropertyMap.getInstance().getProperty("config", "mail_host"));

		Authenticator auth = new Authenticator() {
			protected PasswordAuthentication getPasswordAuthentication() {
				return new PasswordAuthentication(PropertyMap.getInstance().getProperty("config", "mail_sender"), PropertyMap.getInstance().getProperty("config", "mail_password"));
			}
		};

		Session session = Session.getDefaultInstance(props, auth);
		try {
			MimeMessage message = new MimeMessage(session);
			message.setFrom(new InternetAddress(PropertyMap.getInstance().getProperty("config", "mail_sender")));
			message.setSubject(title);
			message.setRecipient(Message.RecipientType.TO, new InternetAddress(address));
			//message.setText(content);
			message.setContent(content, "text/html; charset=utf-8");

			Transport.send(message);
		} catch (Throwable e) {
			System.out.println(e.toString());
		}
	}

}
