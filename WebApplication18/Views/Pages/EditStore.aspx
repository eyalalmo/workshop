    <%@ Page  Title="Edit Stores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditStore.aspx.cs"  Inherits="WebApplication18.Views.Pages.EditStore" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
     

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <script type="text/javascript">
        $(document).ready(function () {
             var doc = document.getElementById('allStores');
         var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
            jQuery.ajax({
                type: "GET",
                url: baseUrl+"/api/user/getAllStores",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    console.log(response);
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
                                + "<p></p><a href=\"" + baseUrl + "/Store?storeId=" + storeId + "\"  class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Manage Store Permissions</a>" 
                                + "<a href=\"" + baseUrl + "/Store?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >View Store info</a>" 
                                + "<p></p><a href=\"" + baseUrl + "/Store?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Delete Store</a>" 
                                + "<p></p><a href=\"" + baseUrl + "/ManageStaff?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Staff Managment</a>"
                                +"<p></p><a href=\""+baseUrl+"/Store?storeId="+storeId+"\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Make It Avilable</a>"
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



