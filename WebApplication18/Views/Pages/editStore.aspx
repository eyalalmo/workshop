    <%@ Page  Title="Edit Stores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditStore.aspx.cs"  Inherits="WebApplication18.Views.Pages.EditStore" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
     

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <script type="text/javascript">
         var getUrl = window.location;
          var baseUrl = getUrl.protocol + "//" + getUrl.host
        $(document).ready(function () {
            var doc = document.getElementById('allStores');
           
        
            jQuery.ajax({
                type: "GET",
                url: baseUrl+"/api/user/getAllStores",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var responsJ= JSON.parse(response);
                    if (responsJ !== "fail") {
                      
                        var HTML = "";
                        for (i = 0; i < responsJ.length ; i++) {        
                            var storeId = responsJ[i].storeID;
                            var storeName = responsJ[i].storeName;
                            var description = responsJ[i].description;
                            var active = responsJ[i].active;

                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			                <h2>Store Name:  `+ storeName+`</h2 >
		                    <h4>Store ID:  `+ storeId +`</h4 >
                             <h4>description:  `+ description +`</h4 >
                            <h4>IsActive:  `+ active+`</h4 >

			                    <div class="detail">
			                    <p></p>`
                                + "<a href=\"" + baseUrl + "/Store?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Manage Store Products</a>" 
                                 + "<p></p><a href=\"" + baseUrl + "/StoreDiscount?storeID=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Edit Store Discounts</a>" 
       
                                 + "<p></p><a href=\"" + baseUrl + "/ViewStore?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >View Store Products</a>" 
 
                               
                                + "<p></p><a href=\"" + baseUrl + "/ManageStaff?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Staff Managment</a>"
                                +"<p></p><a href=\""+baseUrl+"/Store?storeId="+storeId+"\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Make It Avilable</a>"
                                + "<p></p><a href=\"" + baseUrl + "/AddProduct?storeId=" + storeId + "\"    class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Add Product</a>" 
			                 +`   </div>
		                    </div>
		                    </div>`
                        }
                        doc.innerHTML += HTML;
                    }
                    else {
                        alert(responsJ);
                    }
                },
                error: function (response) {
                    //alert("Lost DB connection");
                    window.location.href = baseUrl+"/index";
                }
            });
            

        });
        function manage(id) {
            event.preventDefault();
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/IsEP?storeId=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
          
                    if (response === true) {
                         window.location.href = baseUrl + "/Store?storeId=" + id;
                    }
                    else {
                        alert("you dont have the permissions to edit products");
                    }
                },
                 error: function (response) {
                     alert("dddd")
                    window.location.href = baseUrl + "/Default";
                }
            });
          
            }
    </script>
 </asp:Content>



