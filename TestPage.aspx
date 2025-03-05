<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="Route.TestPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Page</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        /* Navbar */
        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }
        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        /* Sidebar */
        .sidebar {
            position: fixed;
            top: 50px;
            left: -250px;
            width: 250px;
            height: calc(100vh - 50px);
            /*background-color: rgba(34, 34, 34, 0.95);*/
            color: white;
            overflow-y: auto;
            transition: left 0.3s ease-in-out;
            z-index: 1100;
            padding: 15px;
        }
        .sidebar.open {
            left: 0;
        }
        .sidebar ul {
            padding: 0;
            list-style: none;
        }
        .sidebar .nav-item {
            padding: 8px 0;
        }
        .sidebar .nav-link {
            color: white;
            text-decoration: none;
            display: block;
            padding: 10px;
            transition: background 0.3s;
        }
        .sidebar .nav-link:hover {
            background: rgba(255, 255, 255, 0.2);
        }

        /* Overlay */
        .overlay {
            display: none;
            position: fixed;
            top: 50px;
            left: 0;
            width: 100%;
            height: calc(100vh - 50px);
            background: rgba(0, 0, 0, 0.5);
            z-index: 1050;
        }

        /* Content */
        .content {
            margin-top: 50px;
            padding: 20px;
            transition: opacity 0.3s ease-in-out;
        }
        .sidebar.open ~ .content {
            opacity: 0.7;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>

    <script>
        function toggleSidebar() {
            $(".sidebar").toggleClass("open");
            $(".overlay").toggle();
        }

        $(document).ready(function () {
            $(".overlay").click(function () {
                $(".sidebar").removeClass("open");
                $(this).hide();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="overlay"></div>

        <!-- Navbar -->
        <nav class="navbar navbar-white bg-white">
            <button class="navbar-toggler" type="button" onclick="toggleSidebar()">
                <span class="navbar-toggler-icon"></span>
            </button>
            <span class="brand">SYNAPSE</span>
        </nav>
        <hr />

        <div class="container body-content">
            <!-- Sidebar -->
            <aside class="sidebar">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Home">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Create">Create</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Map">Map</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Transfer">Transfer</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/NewGeo">NewGeo</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/BeatRealignment">Beat Realignment</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/SOBeatCreation">SO Beat Creation</a>
                    </li>
                </ul>
            </aside>

            <!-- Main Content -->
            <main class="content">
                <h2 style="text-align: center; margin-top: 20px;">Welcome</h2>

                <div class="headtag">
                    <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-right: 20px;"></asp:Label>
                </div>
                <table style="width: 100%; font-family: Calibri; font-size: small">
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lbl_msg" Text="" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large" BackColor="LightPink"></asp:Label>
                        </td>
                    </tr>
                </table>
            </main>

            <!-- Notification Section -->
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>
    </form>
</body>
</html>
