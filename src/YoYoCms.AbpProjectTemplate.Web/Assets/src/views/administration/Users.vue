<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-users-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }

        .el-table {
            overflow: visible !important;

            * {
                overflow: visible !important;
            }
        }
    }
</style>

<template>
    <article class="administration-users-container">
        <article class="search">
            <section>
                <i>搜索</i>
                <el-input placeholder="输入任意搜索条件进行搜索" v-model="fetchParam.filter"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
        </article>

        <el-table class="data-table" v-loading="loadingData"
                  :data="data"
                  :fit="true"
                  border>
            <!--<el-table-column type="selection"></el-table-column>-->
            <el-table-column
                    width="100"
                    label="操作">
                <template scope="scope">
                    <div class="btn-group">
                        <button type="button" class="btn btn-info dropdown-toggle" data-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false">
                            操作 <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu">
                            <li @click="dialogPermissionTree.isShow = true;dialogPermissionTree.userid = scope.row.id">
                                <a href="javascript:void(0);">权限</a></li>
                            <li @click="dialogEdit.isShow=true;dialogEdit.user=scope.row"><a href="javascript:void(0);">修改</a></li>
                            <li><a href="javascript:void(0);">Something else here</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="javascript:void(0);">Separated link</a></li>
                        </ul>
                    </div>
                </template>
            </el-table-column>
            <el-table-column
                    min-width="120"
                    prop="userName"
                    label="用户名">
            </el-table-column>
            <el-table-column
                    min-width="120"
                    prop="name"
                    label="名字">
            </el-table-column>
            <el-table-column
                    width="120"
                    prop="surname"
                    label="姓氏">
            </el-table-column>
            <el-table-column
                    width="180"
                    label="角色">
                <template scope="scope">
                    <i v-for="item in scope.row.roles">{{item.roleName}}</i>
                </template>
            </el-table-column>
            <el-table-column
                    width="190"
                    prop="emailAddress"
                    label="邮箱地址">
            </el-table-column>
            <el-table-column
                    width="80"
                    label="邮箱地址验证">
                <template scope="scope">
                    <el-tag type="success" v-if="scope.row.isEmailConfirmed">是</el-tag>
                    <el-tag type="gray" v-else>否</el-tag>
                </template>
            </el-table-column>
            <el-table-column
                    width="80"
                    label="激活">
                <template scope="scope">
                    <el-tag type="success" v-if="scope.row.isActive">是</el-tag>
                    <el-tag type="gray" v-else>否</el-tag>
                </template>
            </el-table-column>
            <el-table-column
                    width="190"
                    prop="lastLoginTime"
                    label="上次登录时间">
                <template scope="scope">
                    <i>{{scope.row.lastLoginTime | date2str}}</i>
                </template>
            </el-table-column>
            <el-table-column
                    width="190"
                    label="创建时间">
                <template scope="scope">
                    <i>{{scope.row.creationTime | date2str}}</i>
                </template>
            </el-table-column>
        </el-table>

        <el-pagination class="pagin"
                       @size-change="handleSizeChange"
                       @current-change="handleCurrentChange"
                       :current-page="fetchParam.page"
                       :page-size="fetchParam.page_size"
                       :page-sizes="[15, 30, 60, 100]"
                       layout="sizes,total, prev, pager, next"
                       :total="total">
        </el-pagination>

        <!--权限树弹出框-->
        <PermissionCheckTree v-model="dialogPermissionTree.isShow" :userid="dialogPermissionTree.userid"
                             :onConfirmCb="permissionConfirm"
                             :title="dialogPermissionTree.title"></PermissionCheckTree>


        <DialogEditUser :visiable.sync="dialogEdit.isShow" :user="dialogEdit.user"></DialogEditUser>
    </article>
</template>

<script>
    import userService from '../../services/userService'

    import PermissionCheckTree from '../../components/tree/PermissionCheck.vue'
    import DialogEditUser from './components/DialogEditUser.vue'
    export default {
        data() {
            return {
                loadingData: false,
                data: [], // 表格数据
                total: 0,
                dialogVisible: false,
                selectedIds: [], // 被选中的数据id集合
                fetchParam: {
                    filter: void 0,
                    permission: void 0,
                    role: void 0,
                    sorting: void 0,
                    maxResultCount: 15,
                    skipCount: 0
                },
                dialogPermissionTree: { // 权限列表弹出框
                    isShow: false,
                    title: '',
                    userid: void 0,
                },
                dialogEdit: { // 修改用户信息的弹出框
                    isShow: false,
                    user: void 0,
                },
            }
        },
        created() {
        },
        activated() {
            this.fetchData()
        },
        methods: {
            handleCurrentChange (val) {
                this.fetchParam.page = val
                this.fetchData()
            },
            handleSizeChange (val) {
                this.fetchParam.page_size = val
                this.fetchData()
            },
            async fetchData () {
                this.loadingData = true
                let ret = await userService.getUsers(this.fetchParam).catch(() => {
                    this.loadingData = false
                })
                this.loadingData = false
                this.data = ret.items
                this.total = ret.totalCount
            },
            // 权限弹出框点击确定的回调
            async permissionConfirm (permissions) {
                await userService.updateUserPermissions({
                    id: this.dialogPermissionTree.userid,
                    grantedPermissionNames: permissions
                })
                abp.notify.success('操作成功!', '恭喜')
            },
        },
        components: {PermissionCheckTree, DialogEditUser}
    }
</script>
