<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoreDetails.aspx.cs" Inherits="WebApplication18.Views.Pages.storeDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                url: baseUrl+"/api/store/getAllStores",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    console.log(response);
                    if (response !== "fail") {
                        var responseSplit = response.split(';');
                        var HTML = "";
                        for (i = 0; i < responseSplit.length - 1; i++) {
                            split2 = responseSplit[i].split(',');
                            var storeId = split2[0];
                            var storeName = split2[1];

                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			                <h2>Store Name:  `+ storeName+`</h2 >
		                    <h3>Store ID:  `+ storeId+`</h3 >

			                    <div class="detail">
			                    <p></p>`
                                + "<a href=\"" + baseUrl + "/Store?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >View Store info</a>" 
                                + "<p></p><a href=\"" + baseUrl + "/Store?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Delete Store</a>" 
                                + "<p></p><a href=\"" + baseUrl + "/Store?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Make It Unavilable</a>"
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


