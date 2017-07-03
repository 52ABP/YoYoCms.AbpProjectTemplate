<!--用户列表弹窗-->
<style rel="styleesheet" lang="scss">
    .dialog-userlist--container {
        .el-dialog {
            padding-bottom: 15px;
        }
    }
</style>

<template>
    <el-dialog ref="dialog" class="dialog-userlist--container"
               @open="handleOpen"
               title="选择用户"
               :visible.sync="dialogVisible"
               size="tiny">
        <el-table class="data-table" v-loading="loading"
                  :data="data"
                  :fit="true"
                  border>
            <!--<el-table-column type="selection"></el-table-column>-->
            <el-table-column
                    min-width="120"
                    prop="name"
                    label="用户名">
            </el-table-column>
            <el-table-column
                    width="100"
                    label="操作">
                <template scope="scope">
                    <el-button size="mini" @click="selectUser(scope.row)">
                        <i style="vertical-align: middle;font-size: 12px" class="material-icons" title="选择">done</i>
                        选择
                    </el-button>
                </template>
            </el-table-column>
        </el-table>

        <el-pagination class="pagin"
                       @size-change="handleSizeChange"
                       @current-change="handleCurrentChange"
                       :current-page="fetchParam.page"
                       :page-size="fetchParam.maxResultCount"
                       :page-sizes="[15, 30, 60, 100]"
                       layout="sizes,total, prev, pager, next"
                       :total="total">
        </el-pagination>
    </el-dialog>
</template>

<script>
    export default {
        props: {
            visible: Boolean,
            selectedUserCb: Function, // 确定按钮的回调 参数为 选中的权限id集合
            getUserFn: Function, // 获取用户的方法 返回Promise
        },
        data() {
            return {
                total: 0,
                loading: false,
                fetchParam: getFetchParam(),
                data: [],
                dialogVisible: false
            }
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('update:visible', val)
            }
        },
        async activated() {
        },
        async mounted () {
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
            fetchData () {
                this.getUserFn(this.fetchParam).then((ret) => {
                    this.total = ret.totalCount
                    this.data = ret.items
                })
            },
            handleOpen () {
                this.fetchParam = getFetchParam()
                this.fetchData()
            },
            selectUser (user) {
                this.selectedUserCb(user)
            }
        },
        components: {}
    }

    function getFetchParam() {
        return {
            filter: void 0,
            maxResultCount: 15,
            skipCount: 0,
            page: 1,
        }
    }
</script>

