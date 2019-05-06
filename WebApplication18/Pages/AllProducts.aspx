<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllProducts.aspx.cs" Inherits="WebApplication18.Pages.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
  <div class="container">
    <h1>Products Catalog</h1>   
    <h1> </h1>
      <h1> </h1>
    <button  name="showAll" id="showAll" class="btn btn-primary">Show All Products</button>
    <button  name="searchN" id="searchN" class="btn btn-primary">Search By Name</button>
    <button  name="searchC" id="searchC" class="btn btn-primary">Search By Category</button>
    <div class="container">
	<div id="allProducts" class="row">
        </div>
		</div>
  </div>
</div>
 

    

<script type="text/javascript">

        $(document).ready(function () {
            
            $("#showAll").click(function () {
                event.preventDefault();
                var doc = document.getElementById('allProducts')
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl +"/api/products/getAllProducts",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var HTML;
                        var jsonList = JSON.parse(response);
                        for (i = 0; i < jsonList.length; i++) {
                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			<h3>`+ jsonList[i].productName + `</h3>
<span> Category:`+ jsonList[i].productCategory + `</span>
			<span class="pull-right"> Quantity Left:`+ jsonList[i].quantityLeft + `</span>
			<div class="detail">
			<p>Price:`+ jsonList[i].price + `</p>
		<a href="#" class="btn btn-info">Add To Cart</a>
        <a href="#" class="btn btn-info">Go To Store</a>
			</div>
		</div>
		</div>`
                        }
                        doc.innerHTML += HTML;
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        });

    </script>
    </asp:Content>