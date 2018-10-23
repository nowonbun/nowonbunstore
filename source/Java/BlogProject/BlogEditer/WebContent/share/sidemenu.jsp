<%@page import="common.FactoryJspServlet"%>
<%@page import="bean.CategoryBean"%>
<%@page import="bean.SideMenuBean"%>
<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%SideMenuBean bean = (SideMenuBean) FactoryJspServlet.action("sidemnue.jsp", request, response);%>
<ul id="side-menu" class="collapsible collapsible-accordion">
	<li class="menu-item menu-item-type-post_type menu-item-object-product">
		<a class="collapsible-header waves-effect" id="createCategoryBtn" href="#"> 
			<i class="fa fa-diamond"></i>Create category
		</a>
	</li>
	<%for (CategoryBean menu : bean.getSidemenu()) {%>
		<li class="menu-item menu-item-type-custom menu-item-object-custom current-menu-ancestor current-menu-parent menu-item-has-children">
			<a class="collapsible-header waves-effect arrow-r" 
				<%if(menu.getChild().size() == 0) {%>
					href="list.jsp?idx=<%=menu.getCategoryIdx()%>"
				<%} %>
			>
			<i class="fa fa-download"></i>
			<%=menu.getCategoryName() %>
			<%if(menu.getChild().size() != 0) {%>
				<i class="fa fa-angle-down rotate-icon"></i>
				</a>
				<div class="collapsible-body">
					<ul class="sub-menu">
						<%for(CategoryBean sub: menu.getChild()) {%>												
							<li class="menu-item menu-item-type-custom menu-item-object-custom menu-item-home">
								<a class="collapsible-header waves-effect" href="list.jsp?idx=<%=sub.getCategoryIdx()%>">
									<%=sub.getCategoryName() %>
								</a>
							</li>	
						<%} %>
					</ul>
				</div>
			<%} else {%>
				</a>
			<%} %>
		</li>
	<%}%>
</ul>
