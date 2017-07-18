<!--审计日志-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-auditlog-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }

        .dialog-detail {
            .el-dialog {
                top: 15px !important;
            }
            h2 {
                font-size: 24px;
            }

            .error-status{
                word-break: break-all;
            }

            section {
                font-size: 14px;
                margin: 15px 0 0 0;

                > * {
                    display: inline-block;
                    vertical-align: top;
                }
                em {
                    width: 10em;
                    text-align: right;
                    font-weight: bold;
                    margin-right: 20px;
                }

                pre, i {
                    max-width: 360px;
                    word-break: break-all;
                    word-wrap: break-word;
                }
            }
        }
    }
</style>

<template>
    <article class="administration-auditlog-container">
        <section class="right-top-btnContainer">
            <el-button icon="upload2" @click="exportExcel">{{L('ExportToExcel')}}</el-button>
        </section>

        <article class="search">
            <section>
                <i>{{L('UserName')}}</i>
                <el-input size="small" :placeholder="L('UserName')" v-model="fetchParam.userName"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <i>{{L('DateRange')}}</i>
                <DateRange size="small" :onChange="fetchData" :start.sync="fetchParam.startDate"
                           :end.sync="fetchParam.endDate"></DateRange>
            </section>
            <section>
                <i>{{L('Service')}}</i>
                <el-input size="small" :placeholder="L('Service')" v-model="fetchParam.serviceName"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <i>{{L('Action')}}</i>
                <el-input size="small" :placeholder="L('Action')" v-model="fetchParam.methodName"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <i>{{L('ErrorState')}}</i>
                <el-select size="small" v-model="fetchParam.hasException" :placeholder="L('All')" @change="fetchData" :clearable="true">
                    <el-option :label="L('Success')" :value="false"></el-option>
                    <el-option :label="L('HasError')" :value="true"></el-option>
                </el-select>
            </section>
            <section>
                <i>{{L('Browser')}}</i>
                <el-input size="small" :placeholder="L('Browser')" v-model="fetchParam.browserInfo"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <!--<section>-->
                <!--<el-button size="small" type="primary" icon="search" @click="fetchData">搜索</el-button>-->
            <!--</section>-->
        </article>

        <el-table class="data-table" v-loading="loadingData"
                  :data="data"
                  :fit="true"
                  border>
            <el-table-column width="50">
                <template scope="scope">
                    <i class="material-icons font-bold col-cyan" style="font-size: 14px"
                       v-if="!scope.row.exception">check_circle</i>
                    <i class="material-icons font-bold col-cyan" style="font-size: 14px"
                       v-else>error</i>
                </template>
            </el-table-column>
            <el-table-column width="180" :label="L('Time')">
                <template scope="scope">
                    <i>{{scope.row.executionTime | date2str(true)}}</i>
                </template>
            </el-table-column>
            <el-table-column width="110" prop="userName" :label="L('UserName')"></el-table-column>
            <el-table-column min-width="130" prop="serviceName" :label="L('Service')"></el-table-column>
            <el-table-column min-width="130" prop="methodName" :label="L('Action')"></el-table-column>
            <el-table-column width="100" :label="L('Duration')">
                <template scope="scope">
                    <i>{{scope.row.executionDuration}} ms</i>
                </template>
            </el-table-column>
            <el-table-column prop="clientIpAddress" width="135" :label="L('IpAddress')"></el-table-column>
            <el-table-column prop="clientName" width="160" :label="L('Client')"></el-table-column>
            <el-table-column prop="browserInfo" width="150" :label="L('Browser')"></el-table-column>
            <el-table-column
                    width="100"
                    :label="L('Action')">
                <template scope="scope">
                    <el-button size="small" icon="view" @click="dialogDetail.isShow=true;dialogDetail.model=scope.row"></el-button>
                </template>
            </el-table-column>
        </el-table>

        <el-pagination class="pagin"
                       @size-change="handleSizeChange"
                       @current-change="handleCurrentChange"
                       :current-page="fetchParam.page"
                       :page-size="fetchParam.maxResultCount"
                       :page-sizes="[10, 30, 60, 100]"
                       layout="sizes,total, prev, pager, next"
                       :total="total">
        </el-pagination>

        <!--详情信息的弹出框-->
        <el-dialog class="dialog-detail"
                   :title="L('AuditLogDetail')"
                   :visible.sync="dialogDetail.isShow"
                   size="tiny">
            <article>
                <h2>{{L('UserInformations')}}</h2>
                <article>
                    <section><em>{{L('UserName')}}:</em> {{dialogDetail.model.userName}}</section>
                    <section><em>{{L('IpAddress')}}:</em> {{dialogDetail.model.clientIpAddress}}</section>
                    <section><em>{{L('Client')}}:</em> {{dialogDetail.model.clientName}}</section>
                    <section><em>{{L('Browser')}}:</em> {{dialogDetail.model.browserInfo}}</section>
                </article>

                <h2>{{L('ActionInformations')}}</h2>
                <article>
                    <section><em>{{L('Service')}}:</em> {{dialogDetail.model.serviceName}}</section>
                    <section><em>{{L('Action')}}:</em> {{dialogDetail.model.methodName}}</section>
                    <section><em>{{L('Time')}}:</em> {{dialogDetail.model.executionTime | date2str(true)}}</section>
                    <section><em>{{L('Duration')}}:</em> {{dialogDetail.model.executionDuration}} ms</section>
                    <section><em>{{L('Parameters')}}:</em>
                        <pre lang="js">{{dialogDetail.model.parameters ? JSON.stringify(JSON.parse(dialogDetail.model.parameters), null, '\t') : ''}}</pre>
                    </section>
                </article>

                <h2>{{L('CustomData')}}</h2>
                <article>
                    <section>{{dialogDetail.model.customData || '[NONE]'}}</section>
                </article>

                <h2>{{L('ErrorState')}}</h2>
                <article class="error-status">
                    <section>
                        <i class="material-icons font-bold col-cyan" v-if="!dialogDetail.model.exception "
                           style="font-size: 14px;vertical-align: middle">check_circle</i>
                        <span>{{dialogDetail.model.exception || L('Success')}}</span>
                    </section>
                </article>
            </article>
            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogDetail.isShow = false">关 闭</el-button>
              </span>
        </el-dialog>
    </article>
</template>

<script>
    import auditLogService from '../../services/administration/auditLogService'

    import DateRange from '../../components/date/DateRange.vue'

    export default {
        data() {
            return {
                loadingData: false,
                data: [], // 表格数据
                total: 0,
                fetchParam: { // 筛选条件
                    startDate: new Date(),
                    endDate: new Date(),
                    userName: '',
                    serviceName: '',
                    methodName: '',
                    browserInfo: '',
                    hasException: void 0,
                    sorting: '',
                    maxResultCount: 10,
                    skipCount: 0,
                    page: 1,
                },
                dialogDetail: {
                    isShow: false,
                    model: {}
                }
            }
        },
        created() {
            this.fetchParam.startDate.setHours(0, 0, 0, 0)
            this.fetchParam.endDate.setHours(23, 59, 59, 999)
        },
        activated() {
        },
        methods: {
            handleCurrentChange (val) {
                this.fetchParam.skipCount = Math.abs((val - 1)) * this.fetchParam.maxResultCount
                this.fetchData()
            },
            handleSizeChange (val) {
                this.fetchParam.maxResultCount = val
                this.fetchData()
            },
            async fetchData () {
                try {
                    this.loadingData = true
                    let ret = await auditLogService.getAuditLogs(this.fetchParam)

                    this.data = ret.items
                    this.total = ret.totalCount
                } finally {
                    this.loadingData = false
                    abp.view.setContentLoading(false)
                }
            },
            exportExcel() {
                auditLogService.exportExcel(this.fetchParam)
            }
        },
        components: {DateRange}
    }
</script>
