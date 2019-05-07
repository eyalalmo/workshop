<%@ Page Title="Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Store.aspx.cs"  Inherits="WebApplication18.Views.Pages.Store" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title +" id:  " + ViewData["storeId"] %>
    </h2>
    

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <script type="text/javascript">
        $(document).ready(function () {
            var storeId =<%=ViewData["storeId"]%>;
             var doc = document.getElementById('allStores');
         var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
            jQuery.ajax({
                type: "GET",
                url: baseUrl+"/api/store/getStoreProducts?storeId=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    console.log(response);
                    if (response !== "fail") {
                        var responseSplit = response.split(';');
                        var HTML = "";
                        for (i = 0; i < responseSplit.length - 1; i++) {
                            split2 = responseSplit[i].split(',');
                            var productId = split2[0];
                            var ProductName = split2[1];

                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			                <h3>Product Name :  `+ ProductName +`</h3 >
                            <h3>Product id :  `+ storeId+`</h3 >
		                    <div class="detail">
			                    <p></p>`
		                    +"<a href=\""+baseUrl+"/Store?storeId="+productId+"\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Edit Product info</a>" 
			                 +`   </div>
		                    </div>
		                    </div>`
                         

                        }
                        doc.innerHTML += HTML;;
                        
                    }
                    else {
                        alert("problem");
                    }
                },
                error: function (response) {
                    //alert("Lost DB connection");
                    window.location.href = baseUrl+"/index";
                }
            });
        });
    </script>
 </asp:Content>


