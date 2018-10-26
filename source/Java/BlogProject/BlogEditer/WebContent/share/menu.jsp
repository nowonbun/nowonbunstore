<%@page import="bean.CategoryBean"%>
<%@page import="bean.SideMenuBean"%>
<%@page import="common.FactoryJspServlet"%>
<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%SideMenuBean bean = (SideMenuBean)FactoryJspServlet.action("menu.jsp", request, response);%>
<%for(CategoryBean item : bean.getSidemenu()){%>
<li class="nav-item">
	<a class="nav-link text-uppercase waves-effect waves-light menu-nav-blog" href="<%=item.getCategoryHref()%>"><%=item.getCategoryText() %></a>
</li>
<%} %>

