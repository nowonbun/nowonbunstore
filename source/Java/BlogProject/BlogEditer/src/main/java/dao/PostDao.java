package dao;

import common.Dao;
import model.Post;

public class PostDao extends Dao<Post> {

	protected PostDao() {
		super(Post.class);
	}
}
