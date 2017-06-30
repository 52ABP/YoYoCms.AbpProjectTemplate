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

            section {
                font-size: 14px;
                margin: 15px 0 0 60px;

                > * {
                    display: inline-block;
                    vertical-align: top;
                }
                em {
                    width: 5em;
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
            <el-button icon="upload2" @click="exportExcel">导出到excel</el-button>
        </section>

        <article class="search">
            <section>
                <i>用户名</i>
                <el-input size="small" placeholder="用户名" v-model="fetchParam.userName"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <i>日期</i>
                <DateRange size="small" :onChange="fetchData" :start.sync="fetchParam.startDate"
                           :end.sync="fetchParam.endDate"></DateRange>
            </section>
            <section>
                <i>服务</i>
                <el-input size="small" placeholder="服务" v-model="fetchParam.serviceName"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <i>服务</i>
                <el-input size="small" placeholder="操作" v-model="fetchParam.methodName"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <i>错误状态</i>
                <el-select size="small" v-model="fetchParam.hasException" placeholder="请选择" @change="fetchData" :clearable="true">
                    <el-option label="成功" :value="false"></el-option>
                    <el-option label="失败" :value="true"></el-option>
                </el-select>
            </section>
            <section>
                <i>浏览器</i>
                <el-input size="small" placeholder="浏览器" v-model="fetchParam.browserInfo"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <el-button size="small" type="primary" icon="search" @click="fetchData">搜索</el-button>
            </section>
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
            <el-table-column width="180" label="时间">
                <template scope="scope">
                    <i>{{scope.row.executionTime | date2str(true)}}</i>
                </template>
            </el-table-column>
            <el-table-column width="100" prop="userName" label="用户名"></el-table-column>
            <el-table-column min-width="130" prop="serviceName" label="服务"></el-table-column>
            <el-table-column min-width="130" prop="methodName" label="操作"></el-table-column>
            <el-table-column width="100" label="持续时间">
                <template scope="scope">
                    <i>{{scope.row.executionDuration}} ms</i>
                </template>
            </el-table-column>
            <el-table-column prop="clientIpAddress" width="135" label="ip地址"></el-table-column>
            <el-table-column prop="clientName" width="160" label="客户端"></el-table-column>
            <el-table-column prop="browserInfo" width="150" label="浏览器"></el-table-column>
            <el-table-column
                    width="80"
                    label="操作">
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
                   title="审计日志详情"
                   :visible.sync="dialogDetail.isShow"
                   size="tiny">
            <article>
                <h2>用户信息</h2>
                <article>
                    <section><em>用户名:</em> {{dialogDetail.model.userName}}</section>
                    <section><em>IP地址:</em> {{dialogDetail.model.clientIpAddress}}</section>
                    <section><em>客户端:</em> {{dialogDetail.model.clientName}}</section>
                    <section><em>浏览器:</em> {{dialogDetail.model.browserInfo}}</section>
                </article>

                <h2>操作信息</h2>
                <article>
                    <section><em>服务:</em> {{dialogDetail.model.serviceName}}</section>
                    <section><em>操作:</em> {{dialogDetail.model.methodName}}</section>
                    <section><em>时间:</em> {{dialogDetail.model.executionTime | date2str(true)}}</section>
                    <section><em>持续时间:</em> {{dialogDetail.model.executionDuration}} ms</section>
                    <section><em>参数:</em>
                        <pre lang="js">{{dialogDetail.model.parameters ? JSON.stringify(JSON.parse(dialogDetail.model.parameters), null, '\t') : ''}}</pre>
                    </section>
                </article>

                <h2>[Custom data]</h2>
                <article>
                    <section>{{dialogDetail.model.customData || '[NONE]'}}</section>
                </article>

                <h2>错误状态</h2>
                <article>
                    <section>
                        <i class="material-icons font-bold col-cyan" v-if="!dialogDetail.model.exception "
                           style="font-size: 14px;vertical-align: middle">check_circle</i>
                        <span>{{dialogDetail.model.exception || '成功'}}</span>
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
    import auditLogService from '../../services/auditLogService'

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
            this.fetchData()
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
                }
            },
            exportExcel() {
                auditLogService.exportExcel(this.fetchParam)
            }
        },
        components: {DateRange}
    }
</script>
