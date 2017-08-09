<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-users-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }
    }
</style>

<template>
    <article class="administration-users-container">
        <!--右上角按钮-->
        <section class="right-top-btnContainer">
            <el-button icon="upload2" @click="exportExcel">{{L('ExportToExcel')}}</el-button>
            <el-button v-if="HasP('Create')" type="primary" icon="plus" @click="dialogEdit.isShow=true;dialogEdit.user={}">
                {{L('CreateNewUser')}}
            </el-button>
        </section>

        <!--搜索-->
        <article class="search">
            <section>
                <i>{{L('Search')}}</i>
                <el-input :placeholder="L('Search')" v-model="fetchParam.filter"
                          @keyup.enter.native="fetchData"></el-input>
            </section>
            <section>
                <i>{{L('Permissions')}}</i>
                <SelPermissionTree v-model="fetchParam.permission" :onChange="fetchData"></SelPermissionTree>
            </section>
            <!--<section>-->
            <!--<el-button type="primary" icon="search" @click="fetchData">搜索</el-button>-->
            <!--</section>-->
        </article>

        <el-table class="data-table" v-loading="loadingData"
                  :data="data"
                  :fit="true"
                  border>
            <!--<el-table-column type="selection"></el-table-column>-->
            <el-table-column
                    min-width="120"
                    prop="userName"
                    :label="L('UserName')">
            </el-table-column>
            <el-table-column
                    min-width="120"
                    prop="name"
                    :label="L('Name')">
            </el-table-column>
            <el-table-column
                    width="120"
                    prop="surname"
                    :label="L('Surname')">
            </el-table-column>
            <el-table-column
                    width="180"
                    :label="L('Roles')">
                <template scope="scope">
                    <i v-for="(item,index) in scope.row.roles">{{item.roleName}}<i
                            v-if="index+1 < scope.row.roles.length">, </i></i>
                </template>
            </el-table-column>
            <el-table-column
                    width="190"
                    prop="emailAddress"
                    :label="L('EmailAddress')">
            </el-table-column>
            <el-table-column
                    width="100"
                    :label="L('EmailConfirm')">
                <template scope="scope">
                    <el-tag type="success" v-if="scope.row.isEmailConfirmed">{{L('Yes')}}</el-tag>
                    <el-tag type="gray" v-else>{{L('No')}}</el-tag>
                </template>
            </el-table-column>
            <el-table-column
                    width="80"
                    :label="L('Active')">
                <template scope="scope">
                    <el-tag type="success" v-if="scope.row.isActive">{{L('Yes')}}</el-tag>
                    <el-tag type="gray" v-else>{{L('No')}}</el-tag>
                </template>
            </el-table-column>
            <el-table-column
                    width="190"
                    prop="lastLoginTime"
                    :label="L('LastLoginTime')">
                <template scope="scope">
                    <i>{{scope.row.lastLoginTime | date2str}}</i>
                </template>
            </el-table-column>
            <el-table-column
                    width="190"
                    :label="L('CreationTime')">
                <template scope="scope">
                    <i>{{scope.row.creationTime | date2str}}</i>
                </template>
            </el-table-column>
            <el-table-column
                    fixed="right"
                    width="110"
                    :label="L('Actions')">
                <template scope="scope">
                    <el-dropdown trigger="click">
                        <el-button type="primary" size="small" class="waves-effect">
                            {{L('Actions')}}
                            <i class="el-icon-caret-bottom el-icon--right"></i>
                        </el-button>
                        <el-dropdown-menu slot="dropdown">
                            <!--权限-->
                            <el-dropdown-item v-if="HasP('ChangePermissions')">
                                <div @click="dialogPermissionTree.isShow = true;dialogPermissionTree.userid = scope.row.id; dialogPermissionTree.title= L('Permissions') + ' - '+scope.row.name">
                                    {{L('Permissions')}}
                                </div>
                            </el-dropdown-item>
                            <!--编辑-->
                            <el-dropdown-item v-if="HasP('Edit')">
                                <div @click="dialogEdit.isShow=true;dialogEdit.user=scope.row">
                                    {{L('Edit')}}
                                </div>
                            </el-dropdown-item>
                            <!--分隔符-->
                            <el-dropdown-item >
                                <div role="separator" class="divider"></div>
                            </el-dropdown-item>
                            <!--解锁-->
                            <el-dropdown-item>
                                <div @click="unlock(scope.$index,scope.row)">{{L('Unlock')}}</div>
                            </el-dropdown-item>
                            <!--删除-->
                            <el-dropdown-item divided v-if="HasP('Delete')">
                                <div @click="del(scope.$index,scope.row)">{{L('Delete')}}</div>
                            </el-dropdown-item>
                        </el-dropdown-menu>
                    </el-dropdown>
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

        <!--权限树弹出框-->
        <DialogUserPermission v-model="dialogPermissionTree.isShow" :userid="dialogPermissionTree.userid"
                              :onConfirmCb="permissionConfirm"
                              :title="dialogPermissionTree.title"></DialogUserPermission>

        <!--修改用户的弹出框-->
        <DialogEditUser :onSaved="dialogUserSave" :visiable.sync="dialogEdit.isShow"
                        :user.sync="dialogEdit.user"></DialogEditUser>
    </article>
</template>

<script>
    import userService from '../../services/userService'

    import DialogUserPermission from '../../components/dialog/UserPermissionTree.vue'
    import DialogEditUser from './components/DialogEditUser.vue'
    import SelPermissionTree from '../../components/select/PermissionTree.vue'
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
                    skipCount: 0,
                    page: 1,
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
                this.loadingData = true
                let ret = await userService.getUsers(Object.assign({}, this.fetchParam, {permission: this.fetchParam.permission ? this.fetchParam.permission.id : null})).catch(() => {
                    this.loadingData = false
                })

                this.data = ret.items
                this.total = ret.totalCount
                this.loadingData = false
                abp.view.setContentLoading(false)
            },
            // 权限弹出框点击确定的回调
            async permissionConfirm (permissions) {
                await userService.updateUserPermissions({
                    id: this.dialogPermissionTree.userid,
                    grantedPermissionNames: permissions || []
                })
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
            },
            async del(index, user) {
                abp.message.confirm(`用户 ${user.name} 将被删除, 是否确认?`, async (ret) => {
                    if (!ret) return
                    this.loadingData = true
                    try {
                        await userService.deleteUser({id: user.id})
                        abp.notify.success('删除成功', '恭喜')
                        this.data.splice(index, 1)
                    } catch (e) {
                        abp.notify.error(e.message, '请求失败')
                    }
                    this.loadingData = false
                })
            },
            // 新增修改 保存成功后回调
            dialogUserSave (type) {
                this.fetchData()
            },
            // 导出到excel
            exportExcel () {
                userService.exportExcel()
            },
            // 解锁
            async unlock(index, item) {
                await userService.unlockUser({id: item.id})
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
            }
        },
        components: {DialogUserPermission, DialogEditUser, SelPermissionTree}
    }
</script>
