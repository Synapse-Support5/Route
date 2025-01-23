<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="Route.TestPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Page</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <%--<link href="Content/bootstrap.css" rel="stylesheet" />--%>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <button type="button" class="form-control" id="btnOpenModal" data-toggle="modal" data-target="#exampleModalCenter">
                Search...
            </button>
        </div>

        <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:TextBox ID="SearchTxt" runat="server" placeholder="Search..." CssClass="form-control mr-3" Style="width: 50%;" />
                        <div class="form-control d-flex align-items-center justify-content-center mr-3" style="width: 30%;">
                            <asp:RadioButton ID="rbActive" runat="server" GroupName="Status" Text="Active" CssClass="radio-option mr-3" />
                            <asp:RadioButton ID="rbInactive" runat="server" GroupName="Status" Text="Inactive" CssClass="radio-option" />
                        </div>
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanelFetch" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnFetch" runat="server" CssClass="btn btn-primary" Text="Fetch" OnClick="btnFetch_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="modal-body" style="max-height: 400px; overflow-y: auto;">
                        
                        <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="SearchModalGrid" runat="server" CssClass="table table-bordered form-group"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="Distcode" HeaderText="Dist. Code" />
                                        <asp:BoundField DataField="RouteCode" HeaderText="Route Code" />
                                        <asp:BoundField DataField="RtrCode" HeaderText="Rtr Code" />
                                        <asp:BoundField DataField="UrCode" HeaderText="Ur Code" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
