<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShoppingBasket.aspx.cs" Inherits="WebApplication18.Pages.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %> ShoppingBasket
    </h2>

<!------ Include the above in your HEAD tag ---------->

 <div class="container">
	<div class="row">
		<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		<div class="my-list">
			<h3>HP Core i3 6th Gen</h3>
			<span>RS:45K</span>
			<div class="detail">
			<a href="#" class="btn btn-info">Remove</a>
			<a href="#" class="btn btn-info">Detail</a>
			</div>
		</div>
		</div>
		
		</div>
    </div>
</asp:Content>
