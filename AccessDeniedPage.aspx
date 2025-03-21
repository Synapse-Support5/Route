﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessDeniedPage.aspx.cs" Inherits="Route.AccessDeniedPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Access Denied</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />

    <style>
        body {
            background-color: white;
        }

        .access-denied-container {
            text-align: center;
            margin-top: 100px;
        }

        .access-denied-text {
            font-size: 24px;
            font-weight: bold;
            color: #333; /* Dark gray */
        }

        .padlock-icon {
            font-size: 48px;
            color: #333;
        }

        .additional-text {
            font-size: 16px;
            color: #666;
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            #btnOpenModal {
                width: 100%;
            }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

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
            transition: opacity 0.3s ease-in-out;
        }

        .sidebar.open ~ .content {
            opacity: 0.7;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>

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
    <form id="form2" runat="server">
        <div class="overlay"></div>

        <%--<nav class="navbar navbar-expand-lg navbar-white bg-white">
            <div class="container">
                <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item">
                            <a id="HomeLink" class="nav-link" runat="server" href="~/Home">Home</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>--%>

        <!-- Navbar -->
        <nav class="navbar navbar-white bg-white">
            <button class="navbar-toggler" type="button" onclick="toggleSidebar()">
                <span class="navbar-toggler-icon"></span>
            </button>
            <%--<span class="brand">SYNAPSE</span>--%>
            <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
        </nav>
        <hr />

        <div class="container body-content">

            <aside class="sidebar">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Home" onclick="showLoading()">Home</a>
                    </li>
                </ul>
            </aside>
            <!-- Main Content -->
            <main class="content">
                <div class="access-denied-container">
                    <div class="access-denied-text">ACCESS DENIED</div>
                    <div class="padlock-icon">🔒</div>
                    <div class="additional-text">
                        YOU DON'T HAVE ACCESS TO VIEW THIS PAGE. IF YOU SHOULD BE ABLE TO ACCESS THIS PAGE, PLEASE CONTACT OUR TEAM.
                    </div>
                </div>
            </main>
        </div>

    </form>

</body>
</html>
