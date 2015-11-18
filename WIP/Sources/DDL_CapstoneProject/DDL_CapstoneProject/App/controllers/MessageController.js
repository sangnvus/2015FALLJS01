"use strict";

app.controller('MessageController',
    function ($scope, $location, $rootScope, $route, toastr, conversations,
        MessageService, CommmonService, UserService, DTOptionsBuilder, DTColumnDefBuilder, SweetAlert) {
        //Todo here

        //Atrributes
        $scope.ListConversations = conversations.data.Data;
        $scope.Inbox = ($route.current.params.list === "") || ($route.current.params.list === undefined) ? "all" : $route.current.params.list;
        $scope.NumberNewMessage = $rootScope.UserInfo.NumberNewMessage;
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
                            if ($scope.Inbox === "all" || $scope.Inbox === "sent") {
                                $scope.ListConversations.unshift(result.data.Data);
                            }
                            toastr.success("Đã gửi");
                        } else {
                            var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                            if (a) {
                                $scope.Error = result.data.Message;
                                toastr.error($scope.Error, 'Lỗi');
                            }
                        }
                    },
                    function (error) {
                        toastr.error('Lỗi');
                    });
            } else {
                toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo');
            }
        }

        function getListConversation() {
            var promiseGet = MessageService.getListConversations();

            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListConversations = result.data.Data;
                        $scope.Inbox = "all";
                    } else {
                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        if (a) {
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    }
                },
                function (error) {
                    toastr.error('Lỗi');
                });
        }

        function getListReceivedConversation() {
            var promiseGet = MessageService.getListReceivedConversations();

            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListConversations = result.data.Data;
                        $scope.Inbox = "inbox";
                    } else {
                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        if (a) {
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    }
                },
                function (error) {
                    toastr.error('Lỗi');
                });
        }

        function getListSentConversation() {
            var promiseGet = MessageService.getListSentConversations();

            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListConversations = result.data.Data;
                        $scope.Inbox = "sent";
                    } else {
                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        if (a) {
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    }
                },
                function (error) {
                    toastr.error('Lỗi');
                });
        }

        $scope.changeInboxSent = function (value) {
            $scope.selection = [];
            addSelected(false);
            if (value === "all") {
                getListConversation();
            } else if (value === "inbox") {
                getListReceivedConversation();
            } else {
                getListSentConversation();
            }
        }

        $scope.Delete = function () {
            if ($scope.selection.length > 0) {
                SweetAlert.swal({
                    title: "Xóa tin nhắn",
                    text: "Bạn có chắc chắn muốn xóa tin nhắn này không?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Có, tôi chắc chắn",
                    cancelButtonText: "Không",
                    closeOnConfirm: true
                },
                   function (isConfirm) {
                       if (isConfirm) {

                           var promise = MessageService.DeleteMessages($scope.selection);
                           promise.then(
                               function (result) {
                                   if (result.data.Status === "success") {
                                       toastr.success('Đã xóa');
                                       if ($scope.Inbox === "all") {
                                           getListConversation();
                                       } else if ($scope.Inbox === "inbox") {
                                           getListReceivedConversation();
                                       } else {
                                           getListSentConversation();
                                       }
                                       $scope.selection = [];
                                   } else {
                                       var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                                       if (a) {
                                           $scope.Error = result.data.Message;
                                           toastr.error($scope.Error, 'Lỗi');
                                       }
                                   }
                               },
                               function (error) {
                                   toastr.error('Lỗi');
                               });
                       }
                   });
            } else {
                toastr.warning("Bạn chưa chọn tin nhắn để xóa");
            }
        }

        $scope.getUserName = function (val) {
            var promise = UserService.getUserName(val);
            return promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        return result.data.Data;
                    } else {
                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        if (a) {
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    }
                },
                function (error) {
                    toastr.error('Lỗi');
                });
        };
    });