﻿"use strict";

app.controller('AdminMessageController',
    function ($scope, $location, $rootScope, $route, toastr, conversations,
        UserService,MessageService, CommmonService, DTOptionsBuilder, DTColumnDefBuilder) {

        $scope.ListConversations = conversations.data.Data;
        $scope.Sent = $route.current.params.list === "sent" ? true : false;
        $scope.checkAll = false;
        $scope.selection = [];

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(50)
        .withOption('bLengthChange', false)
        .withOption('order', [3, 'desc'])
        .withBootstrap();

        // Add selected column to table with value true/false
        function addSelected(bool) {
            for (var i = 0; i < $scope.ListConversations.length; i++) {
                $scope.ListConversations[i].Selected = bool;
            }
            if (!bool) {
                $scope.checkAll = false;
            }
        }

        // Call function addSelected
        addSelected(false);

        // watch ListConversations for changes
        $scope.$watch('ListConversations|filter:{Selected:true}', function (nv) {
            $scope.selection = nv.map(function (conversation) {
                return conversation.ConversationID;
            });
        }, true);

        // Check all checkbox
        $scope.checkAllClick = function (boolAll) {
            if (boolAll) {
                addSelected(true);
            } else {
                addSelected(false);
            }
        }

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable()
        ];

        function getListReceivedConversation() {
            var promiseGet = MessageService.getListReceivedConversations();

            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListConversations = result.data.Data;
                        $scope.Sent = false;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        function getListSentConversation() {
            var promiseGet = MessageService.getListSentConversations();

            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListConversations = result.data.Data;
                        $scope.Sent = true;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        $scope.changeInboxSent = function (value) {
            $scope.selection = [];
            addSelected(false);
            if (value === "inbox") {
                getListReceivedConversation();
            } else {
                getListSentConversation();
            }
        }

        $scope.Delete = function () {
            var promise = MessageService.DeleteMessages($scope.selection);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        toastr.success('Đã xóa thành công!');
                        if ($scope.Sent === true) {
                            getListSentConversation();
                        } else {
                            getListReceivedConversation();
                        }
                        $scope.selection = [];
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }

        $scope.getUserName = function (val) {
            var promise = UserService.getUserName(val);
            return promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        return result.data.Data;
                    }
                },
                function (error) {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        };

        // Functions reset new message form
        function resetNewMessageModel() {
            $scope.NewMessage = {
                ToUser: "",
                Title: "",
                Content: ""
            }
        }
        // call function
        resetNewMessageModel();

        // Function request 
        $scope.sendMessage = function () {
            if ($scope.NewMessage.Content.trim() !== "") {
                var promisePost = MessageService.sendMessage($scope.NewMessage);
                promisePost.then(
                    function (result) {
                        if (result.data.Status === "success") {
                            $('#newMesageModal').modal('hide');
                            resetNewMessageModel();
                            if ($scope.Sent) {
                                $scope.ListConversations.unshift(result.data.Data);
                            }
                            toastr.success("Đã gửi");
                        } else {
                            CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    },
                    function (error) {
                        $scope.Error = error.data.Message;
                    });
            } else {
                toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo');
            }
        }
    });