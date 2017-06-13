(function () {
    appModule.controller('tenant.views.dashboard.index', [
        '$scope', 'abp.services.app.tenantDashboard',
        function ($scope, tenantDashboardService) {
            var vm = this;
            
            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.getMemberActivity = function () {
                tenantDashboardService.getMemberActivity({}).then(function (result) {
                    $("#totalMembersChart").sparkline(result.data.totalMembers, {
                        type: 'bar',
                        width: '100',
                        barWidth: 6,
                        height: '45',
                        barColor: '#F36A5B',
                        negBarColor: '#e02222',
                        chartRangeMin: 0
                    });

                    $("#newMembersChart").sparkline(result.data.newMembers, {
                        type: 'bar',
                        width: '100',
                        barWidth: 6,
                        height: '45',
                        barColor: '#5C9BD1',
                        negBarColor: '#e02222',
                        chartRangeMin: 0
                    });
                });
            };

            vm.getMemberActivity();

            Morris.Area({
                element: 'sales_statistics',
                padding: 0,
                behaveLikeLine: false,
                gridEnabled: false,
                gridLineColor: false,
                axes: false,
                fillOpacity: 1,
                data: [{
                    period: '2011 Q1',
                    sales: 1400,
                    profit: 400
                }, {
                    period: '2011 Q2',
                    sales: 1100,
                    profit: 600
                }, {
                    period: '2011 Q3',
                    sales: 1600,
                    profit: 500
                }, {
                    period: '2011 Q4',
                    sales: 1200,
                    profit: 400
                }, {
                    period: '2012 Q1',
                    sales: 1550,
                    profit: 800
                }],
                lineColors: ['#399a8c', '#92e9dc'],
                xkey: 'period',
                ykeys: ['sales', 'profit'],
                labels: ['Sales', 'Profit'],
                pointSize: 0,
                lineWidth: 0,
                hideHover: 'auto',
                resize: true
            });
        }
    ]);
})();