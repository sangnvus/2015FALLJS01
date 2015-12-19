"use strict";

app.controller('AdminCategoryController',
    function ($scope, $rootScope, toastr, listcategory, AdminCategoryService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder, SweetAlert) {
        //Todo here.
        $scope.ListCategory = listcategory.data.Data;
        $scope.NewCategory = {
            Name: null,
            Description: null
        };

        $scope.EditIndex = null;

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [6, 'desc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(7).notSortable()
        ];

        $scope.changeCategoryStatus = function (id, index) {
            var promise = AdminCategoryService.changeCategoryStatus(id);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListCategory[index].IsActive = result.data.Data.IsActive;
                        if ($scope.ListCategory[index].IsActive) {
                            toastr.success("Đã mở khóa");
                        } else {
                            toastr.success("Đã khóa");
                        }
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

        // Functions reset new message form
        $scope.resetNewCategoryForm = function resetNewCategoryModel(NewCategoryForm) {
            $scope.NewCategory = {
                Name: null,
                Description: null
            }
            NewCategoryForm.$setPristine(true);
        }

        // Add new category
        $scope.addNewCategory = function () {
            var promisePost = AdminCategoryService.addNewCategory($scope.NewCategory);
            promisePost.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListCategory.push(result.data.Data);
                        toastr.success("Tạo danh mục thành công");
                        $('#newCategoryModal').modal('hide');
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

        // Delete category
        $scope.deleteCategory = function (id, index) {
            SweetAlert.swal({
                title: "Xóa danh mục",
                text: "Bạn có chắc chắn muốn xóa danh mục này không?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có, tôi chắc chắn",
                cancelButtonText: "Không",
                closeOnConfirm: true
            },
                    function (isConfirm) {
                        if (isConfirm) {
                            var promise = AdminCategoryService.deleteCategory(id);
                            promise.then(
                                function (result) {
                                    if (result.data.Status === "success") {
                                        $scope.ListCategory.splice(index, 1);
                                        toastr.success("Xóa thành công");
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
        }

        $scope.showEditDialog = function (index, EditCategoryForm) {
            $scope.EditIndex = index;
            var category = $scope.ListCategory[index];

            $scope.EditCategory = {
                Name: category.Name,
                Description: category.Description,
                CategoryID: category.CategoryID,
            }

            EditCategoryForm.$setPristine(true);

            $("#EditCategoryModal").modal('show');

        }

        // Edit category
        $scope.editCategory = function () {
            var promise = AdminCategoryService.editCategory($scope.EditCategory);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListCategory[$scope.EditIndex] = result.data.Data;
                        toastr.success("Sửa thành công");
                        $('#EditCategoryModal').modal('hide');
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