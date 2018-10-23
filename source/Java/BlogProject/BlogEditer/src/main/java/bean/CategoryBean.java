package bean;

import java.util.List;

public class CategoryBean {
	private int categoryIdx;
	private List<CategoryBean> child;
	private String categoryName;
	private int parent;

	public int getCategoryIdx() {
		return categoryIdx;
	}

	public void setCategoryIdx(int categoryIdx) {
		this.categoryIdx = categoryIdx;
	}

	public String getCategoryName() {
		return categoryName;
	}

	public void setCategoryName(String categoryName) {
		this.categoryName = categoryName;
	}

	public int getParent() {
		return parent;
	}

	public void setParent(int parent) {
		this.parent = parent;
	}

	public List<CategoryBean> getChild() {
		return child;
	}

	public void setChild(List<CategoryBean> child) {
		this.child = child;
	}

}
