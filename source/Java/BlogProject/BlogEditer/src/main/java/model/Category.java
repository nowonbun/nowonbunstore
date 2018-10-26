package model;

import java.io.Serializable;
import javax.persistence.*;
import java.util.List;

@Entity
@Table(name = "mst_category")
@NamedQuery(name = "Category.findAll", query = "SELECT c FROM Category c")
public class Category implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	@Column(name = "CATEGORY_CODE")
	private String categoryCode;

	@Column(name = "CATEGORY_NAME")
	private String categoryName;

	private boolean isdeleted;

	private boolean ishome;

	private int sequence;

	private String url;

	@OneToMany(mappedBy = "category", targetEntity = Post.class, fetch = FetchType.LAZY)
	private List<Post> posts;

	public Category() {
	}

	public String getCategoryCode() {
		return this.categoryCode;
	}

	public void setCategoryCode(String categoryCode) {
		this.categoryCode = categoryCode;
	}

	public String getCategoryName() {
		return this.categoryName;
	}

	public void setCategoryName(String categoryName) {
		this.categoryName = categoryName;
	}

	public boolean getIsdeleted() {
		return this.isdeleted;
	}

	public void setIsdeleted(boolean isdeleted) {
		this.isdeleted = isdeleted;
	}

	public boolean getIshome() {
		return this.ishome;
	}

	public void setIshome(boolean ishome) {
		this.ishome = ishome;
	}

	public int getSequence() {
		return this.sequence;
	}

	public void setSequence(int sequence) {
		this.sequence = sequence;
	}

	public String getUrl() {
		return this.url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	public List<Post> getTsnPosts() {
		return this.posts;
	}

	public void setTsnPosts(List<Post> posts) {
		this.posts = posts;
	}

	public Post addTsnPost(Post post) {
		getTsnPosts().add(post);
		post.setCategory(this);

		return post;
	}

	public Post removeTsnPost(Post post) {
		getTsnPosts().remove(post);
		post.setCategory(null);

		return post;
	}

}