<!--组织机构-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-organizationUnits-container {
        //        @extend %content-container;
        background: transparent;
        margin: 0 !important;

        %header {
            border-bottom: 1px solid #eee;
            /*height: ;*/
            font-size: 22px;
            padding-bottom: 8px;
            margin-bottom: 8px;

            .el-button {
                float: right;
            }
        }

        // 左边的树
        .left-tree {
            padding: 15px;

            // 左边头部
            .left-header {
                @extend %header;
            }

            .el-card__body {
                padding: 0;
            }
        }

        .right-list {
            margin-left: 3%;
            padding: 15px;
            .el-card__body {
                padding: 0;
            }
            .right-header {
                @extend %header;
            }

            .el-pagination {
                float: right;
                margin-top: 10px;
            }
        }
    }
</style>

<template>
    <article class="administration-organizationUnits-container row" v-loading="loading">
        <!--左边树-->
        <el-card class="left-tree col-lg-5">
            <div class="left-header">
                {{L('OrganizationTree')}}
                <el-button icon="plus" size="small" @click="showDialogAddOrgan(null)" v-if="HasP('ManageOrganizationTree')">
                    {{L('AddRootUnit')}}
                </el-button>
            </div>
            <JsTree ref="jstree" :treeData="treeData" :onDragStop="dragStop" :onItemClick="orgaizationTreeclick"
                    :contextMenu="treeContextMenu"
                    :plugins="HasP('ManageOrganizationTree') ? ['dnd','types', 'wholerow', 'contextmenu']: []"></JsTree>

            <!--// 添加单元的弹出框-->
            <el-dialog
                    :title="dialogAddOrgan.title"
                    :visible.sync="dialogAddOrgan.isShow"
                    size="tiny">
                <el-input :placeholder="L('Name')" v-model="dialogAddOrgan.displayName"></el-input>
                <span slot="footer" class="dialog-footer">
            <el-button @click="dialogAddOrgan.isShow = false">{{L('Cancel')}}</el-button>
            <el-button type="primary" @click="saveOrgan">{{L('Save')}}</el-button>
          </span>
            </el-dialog>
        </el-card>

        <!--右边用户列表-->
        <el-card class="col-lg-6 right-list">
            <div class="right-header">
                [{{L('Members')}}] <i v-show="selectedOrgan.displayName">{{ ':' + selectedOrgan.displayName}}</i>
                <el-button :disabled="!selectedOrgan.id" icon="plus" size="small" @click="dialogUser.isShow=true">
                    {{L('AddMember')}}
                </el-button>
            </div>
            <el-table class="data-table" v-loading="userList.loading"
                      :data="userList.data"
                      :fit="true"
                      border>
                <el-table-column
                        min-width="120"
                        :label="L('UserName')">
                    <template scope="scope">
                        <i>{{scope.row.userName}}</i>
                    </template>
                </el-table-column>
                <el-table-column
                        width="150"
                        prop="lastLoginTime"
                        :label="L('AddedTime')">
                    <template scope="scope">
                        <i>{{scope.row.addedTime | date2str}}</i>
                    </template>
                </el-table-column>
                <el-table-column
                        width="80"
                        :label="L('Action')">
                    <template scope="scope">
                        <i style="cursor:pointer;" class="material-icons" :title="L('Delete')"
                           @click="delUser(scope.$index,scope.row)">delete_forever</i>
                    </template>
                </el-table-column>
            </el-table>

            <el-pagination class="pagin" v-show="userList.total > 0"
                           @size-change="handleSizeChange"
                           @current-change="handleCurrentChange"
                           :current-page="userList.fetchParam.page"
                           :page-size="userList.fetchParam.maxResultCount"
                           :page-sizes="[15, 30, 60, 100]"
                           layout="sizes,total, prev, pager, next"
                           :total="userList.total">
            </el-pagination>
            <DialogUserlist :selectedUserCb="selectedUser" :visible.sync="dialogUser.isShow"
                            :getUserFn="getUsers"></DialogUserlist>
        </el-card>
    </article>
</template>

<script>
    import JsTree from '../../components/tree/JsTree.vue'
    import organizationUnitService from '../../services/administration/organizationUnitService'
    import commonService from '../../services/commonLookupService'
    import DialogUserlist from '../../components/dialog/UserList.vue'
    export default {
        data() {
            return {
                orignTreeData: [],
                treeData: [],
                loading: false,
                // 添加组织机构
                dialogAddOrgan: {
                    isShow: false,
                    parentId: 0,
                    displayName: void 0,
                    type: 'add', // 'add' 或 'edit'
                },
                selectedOrgan: {},
                userList: {
                    id: void 0,
                    total: 0,
                    data: [],
                    loading: false,
                    fetchParam: resetUserListFetchParam()
                },
                // 用户列表弹出框
                dialogUser: {
                    isShow: false
                }
            }
        },
        created() {
        },
        async activated() {
        },
        methods: {
            async fetchData () {
                this.loading = true
                this.orignTreeData = (await organizationUnitService.getOrganizationUnits()).items
                this.initTree()
                this.loading = false
                abp.view.setContentLoading(false)
            },
            initTree() {
                this.orignTreeData.map((item) => {
                    item.text = `${item.displayName} (${item.memberCount})`
                    item.parent = item.parentId || '#'
                    item.state = {opened: true}
                })
                this.treeData = this.orignTreeData
                this.$refs.jstree.init()
            },
            //
            dragStop (id, newParentId) {
                abp.message.confirm(lang.L('OrganizationUnitMoveConfirmMessage'), async (ret) => {
                    if (!ret) {
                        this.$refs.jstree.init()
                        return
                    }

                    try {
                        this.loading = true
                        await organizationUnitService.moveOrganizationUnit({id, newParentId})
                        abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
                    } finally {
                        this.loading = false
                    }
                })
            },
            // 保存添加和修改操作
            async saveOrgan() {
                if (this.dialogAddOrgan.type === 'add') await organizationUnitService.createOrganizationUnit(this.dialogAddOrgan)
                else await organizationUnitService.updateOrganizationUnit(this.dialogAddOrgan)
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
                this.dialogAddOrgan.isShow = false
                this.fetchData()
            },
            // 显示添加或修改组织单元的弹出框
            showDialogAddOrgan (pid, displayName = null, type = 'add', id) {
                this.dialogAddOrgan.parentId = pid
                this.dialogAddOrgan.displayName = displayName
                this.dialogAddOrgan.isShow = true
                this.dialogAddOrgan.type = type
                this.dialogAddOrgan.id = id

                if (type === 'add') this.dialogAddOrgan.title = lang.L('NewOrganizationUnit') // !pid ? '添加根单元' : `添加子单元`
                else this.dialogAddOrgan.title = `${lang.L('Edit')}: ${displayName}`
            },
            // 组织机构的右键菜单
            treeContextMenu(node) {
                let _this = this
                let items = {
                    addSubUnit: {
                        label: lang.L('AddSubUnit'),
                        action(data) {
                            _this.showDialogAddOrgan(node.id, null, 'add')
                        },
                        icon: 'el-icon-plus'
                    },
                    editUnit: {
                        label: lang.L('Edit'),
                        action(data) {
                            _this.showDialogAddOrgan(null, node.displayName, 'edit', node.id)
                        },
                        icon: 'el-icon-edit'
                    },
                    delUnit: {
                        label: lang.L('Delete'),
                        action(data) {
                            abp.message.confirm(lang.L('OrganizationUnitDeleteWarningMessage', node.displayName), async (ret) => {
                                if (!ret) return
                                await organizationUnitService.deleteOrganizationUnit({id: node.id})
                                _this.fetchData()
                                abp.notify.success(lang.L('SuccessfullyDeleted'), lang.L('Success'))
                            })
                        },
                        icon: 'el-icon-delete2'
                    },
                }
                return items
            },
            //  =================================右边 用户列表部分=====================================
            // 获取用户列表数据
            async fetchData4Users () {
                let ret = await organizationUnitService.getOrganizationUnitUsers(this.userList.fetchParam)
                this.userList.data = ret.items
                this.userList.total = ret.totalCount
                this.userList.loading = false
            },
            // 权限树点击
            async orgaizationTreeclick (item) {
                this.userList.loading = true
                this.selectedOrgan = item
                this.userList.fetchParam = resetUserListFetchParam()
                this.userList.fetchParam.id = item.id
                this.fetchData4Users()
            },
            handleCurrentChange (val) {
                this.userList.fetchParam.skipCount = Math.abs((val - 1)) * this.userList.fetchParam.maxResultCount
                this.fetchData4Users()
            },
            handleSizeChange (val) {
                this.userList.fetchParam.maxResultCount = val
                this.fetchData4Users()
            },
            // 获取用户列表的方法
            getUsers(params) {
                return commonService.findUsers(params)
            },
            // 选中用户的回调
            async selectedUser (user) {
                for (let i = 0; i < this.userList.data.length; i++) {
                    let item = this.userList.data[i]
                    if (item.id == user.value) {
                        abp.notify.error(lang.L('UserIsAlreadyInTheOrganizationUnit'), lang.L('Error'))
                        return
                    }
                }
                await organizationUnitService.addUserToOrganizationUnit({
                    userId: user.value,
                    organizationUnitId: this.selectedOrgan.id
                })
                this.dialogUser.isShow = false
                this.fetchData4Users()
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
                this.fetchData()
            },
            // 删除用户
            delUser (index, item) {
                abp.message.confirm(lang.L('RemoveUserFromOuWarningMessage', item.userName, this.selectedOrgan.displayName), async (ret) => {
                    if (!ret) return
                    await organizationUnitService.removeUserFromOrganizationUnit({
                        userId: item.id,
                        organizationUnitId: this.selectedOrgan.id
                    })
                    abp.notify.success(lang.L('SuccessfullyDeleted'), lang.L('Success'))
                    this.userList.data.splice(index, 1)
                    this.fetchData()
                })
            }
        },
        components: {JsTree, DialogUserlist}
    }

    function resetUserListFetchParam() {
        return {
            id: void 0,
            maxResultCount: 15,
            skipCount: 0,
            page: 1,
            sorting: void 0,
        }
    }
</script>
