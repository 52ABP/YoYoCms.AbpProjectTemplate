<!--组织机构-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-organizationUnits-container {
        //        @extend %content-container;
        background: transparent;
        margin: 0 !important;

        // 左边的树
        .left-tree {
            padding: 15px;

            // 左边头部
            .left-header {
                border-bottom: 1px solid #eee;
                /*height: ;*/
                font-size: 22px;
                padding-bottom: 8px;
                margin-bottom: 8px;

                .el-button {
                    float: right;
                }
            }

            .el-card__body {
                padding: 0;
            }
        }

        .right-list {
            margin-left: 3%;
        }
    }
</style>

<template>
    <article class="administration-organizationUnits-container row" v-loading="loading">
        <!--左边树-->
        <el-card class="left-tree col-lg-5">
            <div class="left-header">
                组织结构树
                <el-button icon="plus" type="primary" size="small" @click="dialogRoot.isShow=true">添加根单元</el-button>
            </div>
            <JsTree ref="jstree" :treeData="treeData" :onDragStop="dragStop" :onItemClick="orgaizationTreeclick"
                    :plugins="['dnd','types', 'wholerow']"></JsTree>
        </el-card>

        <!--右边用户列表-->
        <el-card class="col-lg-6 right-list">
            <el-table class="data-table" v-loading="userList.loading"
                      :data="userList.data"
                      :fit="true"
                      border>
                <el-table-column
                        min-width="120"
                        label="角色名称">
                    <template scope="scope">
                        <i>{{scope.row.displayName}}</i>
                        <el-tooltip content="不能删除系统角色" placement="top" v-if="scope.row.isStatic">
                            <el-tag type="success">系统</el-tag>
                        </el-tooltip>

                        <el-tooltip content="新用户将默认拥有此角色" placement="top" v-if="scope.row.isDefault">
                            <el-tag type="gray">默认</el-tag>
                        </el-tooltip>
                    </template>
                </el-table-column>
                <el-table-column
                        width="190"
                        prop="lastLoginTime"
                        label="创建时间">
                    <template scope="scope">
                        <i>{{scope.row.creationTime | date2str}}</i>
                    </template>
                </el-table-column>
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
                                <li @click="dialogEdit.isShow=true;dialogEdit.role=scope.row">
                                    <a>修改</a>
                                </li>
                            </ul>
                        </div>
                    </template>
                </el-table-column>
            </el-table>
        </el-card>

        <!--// 添加根单元的弹出框-->
        <el-dialog
                title="添加根单元"
                :visible.sync="dialogRoot.isShow"
                size="tiny">
            <el-input placeholder="名字"></el-input>
            <span slot="footer" class="dialog-footer">
            <el-button @click="dialogRoot.isShow = false">取 消</el-button>
            <el-button type="primary" @click="saveRoot">确 定</el-button>
          </span>
        </el-dialog>
    </article>
</template>

<script>
    import JsTree from '../../components/tree/JsTree.vue'
    import organizationUnitService from '../../services/organizationUnitService'
    export default {
        data() {
            return {
                treeData: [],
                loading: false,
                dialogRoot: {
                    isShow: false
                },
                userList: {
                    data: [],
                    loading: false,
                    fetchParam: resetUserListFetchParam()
                }
            }
        },
        created() {
        },
        async activated() {
            this.fetchData()
        },
        methods: {
            async fetchData () {
                this.loading = true
                let treeData = (await organizationUnitService.getOrganizationUnits()).items
                treeData.map((item) => {
                    item.text = `${item.displayName} (${item.memberCount})`
                    item.parent = item.parentId || '#'
                    item.state = {opened: true}
                })
                this.treeData = treeData
                this.$refs.jstree.init()
                this.loading = false
            },
            //
            dragStop (id, newParentId) {
                abp.message.confirm('是否确认移动?', async (ret) => {
                    if (!ret) {
                        this.$refs.jstree.init()
                        return
                    }

                    try {
                        this.loading = true
                        await organizationUnitService.moveOrganizationUnit({id, newParentId})
                        abp.notify.success('操作成功!', '恭喜')
                    } finally {
                        this.loading = false
                    }
                })
            },
            saveRoot() {
            },
            // 权限树点击
            async orgaizationTreeclick (item) {
                this.userList.fetchParam = resetUserListFetchParam()
                this.userList.fetchParam.id = item.id
                organizationUnitService.getOrganizationUnitUsers(this.userList.fetchParam)
            }
        },
        components: {JsTree}
    }

    function resetUserListFetchParam() {
        return {
            id: void 0,
            maxResultCount: 10,
            skipCount: 0,
            page: 1,
            sorting: void 0
        }
    }
</script>
