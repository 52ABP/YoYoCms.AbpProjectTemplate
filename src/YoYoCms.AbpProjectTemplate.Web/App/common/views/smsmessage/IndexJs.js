



// 项目展示地址:"http://www.ddxc.org/"
// 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:43:43. All Rights Reserved.
//<生成时间>2017-07-03T17:43:43</生成时间>

//声明了个控制权名称：common.views.smsMessagelogs.index
//我是角落的白板笔， 有兴趣的 话我们一起研究 吧,qq群：104390185 
(function() {
    appModule.controller("common.views.smsMessagelogs.index",
    [
        "$scope", "$uibModal", "$stateParams", "uiGridConstants", "abp.services.yoyocms.smsMessagelog",
        function($scope, $uibModal, $stateParams, uiGridConstants, smsMessagelogService) {
            //$stateParams 获取的是浏览器后面跟的参数
            var vm = this;

            $scope.$on("$viewContentLoaded",
                function() {
                    //这里应该是当页面加载完毕后，进行信息的初始化
                    //实际会去调用 icheck、select2等js的初始化插件。来渲染页面
                    App.initAjax();
                });
            //告知页面信息已经下载完毕
            vm.loading = false;
            //默认是关闭高级按钮。我们这里用不到。参考
            //  vm.advancedFiltersAreShown = false;
            //获取传递的参数，判断模糊查询字段，是否为空
            vm.filterText = $stateParams.filterText || "";
            //获取Session中的userId
            //   vm.currentUserId = abp.session.userId;
            //制作权限组，进行页面权限的判断 
            vm.permissions = {
                create: abp.auth.hasPermission("Pages.SmsMessagelog.CreateSmsMessagelog"),
                edit: abp.auth.hasPermission("Pages.SmsMessagelog.EditSmsMessagelog"),
                'delete': abp.auth.hasPermission("Pages.SmsMessagelog.DeleteSmsMessagelog")
            };
            //请求参数，默认用于分页
            vm.requestParams = {
                permission: "",
                role: "",
                skipCount: 0,
                //这里是个常量文件，如果你没有找到这个常量文件，那么就手动改成10吧
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

//配置表格信息

            vm.smsMessagelogGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                //这里是个常量文件，如果你没有找到这个常量文件，那么就手动改成[10, 20, 50, 100]吧
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate:
                    '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',

                columnDefs: [
                    {
                        //这里是在处理多语言本地化的问题，如果你这里报错。那么手动修改名字吧。
                        name: app.localize("Actions"),
                        enableSorting: false,
                        width: 120,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="" dropdown-append-to-body>' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' +
                                app.localize("Actions") +
                                ' <span class="caret"></span></button>' +
                                "    <ul uib-dropdown-menu>" +
                                '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editSmsMessagelog(row.entity)">' +
                                app.localize("Edit") +
                                "</a></li>" +
                                '      <li><a ng-if="grid.appScope.permissions.delete" ng-click="grid.appScope.deleteSmsMessagelog(row.entity)">' +
                                app.localize("Delete") +
                                "</a></li>" +
                                "    </ul>" +
                                "  </div>" +
                                "</div>"
                    },
                    //让我么开始循环字段吧。

//id不展示信息
                    {
                        name: app.localize("PhoneNumber"),
                        field: "phoneNumber",
                        //  enableSorting: false,
                        //cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    },
                    {
                        name: app.localize("Content"),
                        field: "content",
                        //  enableSorting: false,
                        //cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    },
                    {
                        name: app.localize("SmsCode"),
                        field: "smsCode",
                        //  enableSorting: false,
                        //cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    },
                    {
                        name: app.localize("InvalidTime"),
                        field: "invalidTime",
                        //  enableSorting: false,
                        //cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    },
                    {
                        name: app.localize("Sucess"),
                        field: "sucess",
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                                '  <span ng-show="row.entity.sucess" class="label label-success">' +
                                app.localize("Yes") +
                                "</span>" +
                                '  <span ng-show="!row.entity.sucess" class="label label-default">' +
                                app.localize("No") +
                                "</span>" +
                                "</div>",
                        minWidth: 80
                    },
                    {
                        name: app.localize("Result"),
                        field: "result",
                        //  enableSorting: false,
                        //cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    },
                    {
                        name: app.localize("IsChecked"),
                        field: "isChecked",
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                                '  <span ng-show="row.entity.isChecked" class="label label-success">' +
                                app.localize("Yes") +
                                "</span>" +
                                '  <span ng-show="!row.entity.isChecked" class="label label-default">' +
                                app.localize("No") +
                                "</span>" +
                                "</div>",
                        minWidth: 80
                    },
                    {
                        name: app.localize("IsNotification"),
                        field: "isNotification",
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                                '  <span ng-show="row.entity.isNotification" class="label label-success">' +
                                app.localize("Yes") +
                                "</span>" +
                                '  <span ng-show="!row.entity.isNotification" class="label label-default">' +
                                app.localize("No") +
                                "</span>" +
                                "</div>",
                        minWidth: 80
                    },
                ],
                // ui-grid进行API注册渲染数据。
                onRegisterApi: function(gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope,
                        function(grid, sortColumns) {
                            if (!sortColumns.length || !sortColumns[0].field) {
                                vm.requestParams.sorting = null;
                            } else {
                                vm.requestParams.sorting = sortColumns[0].field + " " + sortColumns[0].sort.direction;
                            }
                            vm.getPagedsmsMessagelogs();
                        });
                    //配置UI-grid的 API参数
                    gridApi.pagination.on.paginationChanged($scope,
                        function(pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;
                            //执行查询语句
                            vm.getPagedsmsMessagelogs();
                        });
                },
                data: []
            };

            //声明查询方法
            vm.getPagedsmsMessagelogs = function() {
                vm.loading = true;
                smsMessagelogService.getPagedSmsMessagelogsAsync($.extend({ filter: vm.filterText }, vm.requestParams))
                    .then(function(result) {
                        vm.smsMessagelogGridOptions.totalItems = result.data.totalCount;
                        //       console.log(result.data.items);                        
                        vm.smsMessagelogGridOptions.data = result.data.items;
                    }).finally(function() {
                        vm.loading = false;
                    });
            };

            //删除方法
            vm.deleteSmsMessagelog = function(smsMessagelog) {
                abp.message.confirm(
                    app.localize("SmsMessagelogDeleteWarningMessage", smsMessagelog.id),
                    function(isConfirmed) {
                        if (isConfirmed) {
                            smsMessagelogService.deleteSmsMessagelogAsync({
                                id: smsMessagelog.id
                            }).then(function() {
                                vm.getPagedsmsMessagelogs();
                                abp.notify.success(app.localize("SuccessfullyDeleted"));
                            });
                        }
                    }
                );
            };

            //导出为excel文档
            vm.exportToExcel = function() {
                smsMessagelogService.getSmsMessagelogToExcel({})
                    .then(function(result) {
                        app.downloadTempFile(result.data);
                    });
            };


            //编辑功能
            vm.editSmsMessagelog = function(smsMessagelog) {
                //     console.log(smsMessagelog);
                openCreateOrEditSmsMessagelogModal(smsMessagelog.id);
            };
            //创建功能
            vm.createSmsMessagelog = function() {
                openCreateOrEditSmsMessagelogModal(null);
            };

            //打开模态框，进行创建或者编辑的功能操作
            function openCreateOrEditSmsMessagelogModal(smsMessagelogId) {
                var modalInstance = $uibModal.open({
                    templateUrl: "~/App/common/views/smsmessage/createOrEditModal.cshtml",
                    controller: "common.views.smsMessagelogs.createOrEditModal as vm",
                    backdrop: "static",
                    resolve: {
                        smsMessagelogId: function() {
                            return smsMessagelogId;
                        }
                    }
                });

                modalInstance.result.then(function(result) {
                    vm.getPagedsmsMessagelogs();
                });
            }


            //执行查询方法
            vm.getPagedsmsMessagelogs();
        }
    ]);
})();