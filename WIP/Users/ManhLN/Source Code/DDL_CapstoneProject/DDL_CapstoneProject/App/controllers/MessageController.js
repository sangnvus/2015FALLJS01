"use strict";

app.controller('MessageController',
    function ($scope, $location, $rootScope, $route, toastr, conversations, MessageService, CommmonService, UserService, DTOptionsBuilder, DTColumnDefBuilder) {
        //Todo here

        //Atrributes
        $scope.ListConversations = conversations.data.Data;
        $scope.Sent = $route.current.params.list === "sent" ? true : false;
        $scope.Unread = $scope.ListConversations.length;
        $scope.checkAll = false;
        $scope.selection = [];

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('bLengthChange', false)
        .withOption('order', [3, 'desc'])
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable()
        ];

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

        $scope.searchFilter = function (obj) {
            var re = new RegExp($scope.searchText, 'i');
            return !$scope.searchText || re.test(obj.SenderName) || re.test(obj.ReceiverName) || re.test(obj.Title);
        };

        // Check all checkbox
        $scope.checkAllClick = function (boolAll) {
            if (boolAll) {
                addSelected(true);
            } else {
                addSelected(false);
            }
        }

        // Change to message detail
        $scope.showConversationDetail = function (id) {
            $location.path("/user/message/" + id).replace();
        }

        // Function request 
        $scope.sendMessage = function () {
            if ($scope.NewMessage.Content.trim() !== "") {
                var promisePost = MessageService.sendMessage($scope.NewMessage);
                promisePost.then(
                    function (result) {
                        if (result.data.Status === "success") {
                            $('#sentBox').modal('hide');
                            resetNewMessageModel();
                            if ($scope.Sent) {
                                $scope.ListConversations.unshift(result.data.Data);
                            }
                            toastr.success("Gửi tin nhắn thành công!", 'Thông báo!');
                        } else {
                            CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi!');
                        }
                    },
                    function (error) {
                        $scope.Error = error.data.Message;
                    });
            } else {
                toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo!');
            }
        }

        function getListReceivedConversation() {
            var promiseGet = MessageService.getListReceivedConversations();

            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListConversations = result.data.Data;
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
                $scope.Sent = false;
            } else {
                getListSentConversation();
                $scope.Sent = true;
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
    });