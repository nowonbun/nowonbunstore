package model;

import java.io.Serializable;
import javax.persistence.*;
import java.util.List;

@Entity
@NamedQuery(name = "Category.findAll", query = "SELECT c FROM Category c")
public class Category implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private int idx;

	@Column(name = "CATEGORY_NAME")
	private String categoryName;

	@ManyToOne
	@JoinColumn(name = "PARENT")
	private Category parentCategory;

	@OneToMany(mappedBy = "parentCategory", fetch = FetchType.LAZY)
	private List<Category> categories;

	@OneToMany(mappedBy = "category", fetch = FetchType.LAZY)
	private List<Post> posts;

	public Category() {
	}

	public int getIdx() {
		return this.idx;
	}

	public void setIdx(int idx) {
		this.idx = idx;
	}

	public String getCategoryName() {
		return this.categoryName;
	}

	public void setCategoryName(String categoryName) {
		this.categoryName = categoryName;
	}

	public Category getParentCategory() {
		return this.parentCategory;
	}

	public void setParentCategory(Category category) {
		this.parentCategory = category;
	}

	public List<Category> getCategories() {
		return this.categories;
	}

	public void setCategories(List<Category> categories) {
		this.categories = categories;
	}

	public Category addParentCategory(Category category) {
		getCategories().add(category);
		category.setParentCategory(this);

		return category;
	}

	public Category removeParentCategory(Category category) {
		getCategories().remove(category);
		category.setParentCategory(null);

		return category;
	}

	public List<Post> getPosts() {
		return this.posts;
	}

	public void setPosts(List<Post> posts) {
		this.posts = posts;
	}

	public Post addPost(Post post) {
		getPosts().add(post);
		post.setCategory(this);

		return post;
	}

	public Post removePost(Post post) {
		getPosts().remove(post);
		post.setCategory(null);

		return post;
	}

}