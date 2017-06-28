<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-roles-container {
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
    <article class="administration-roles-container">
        <section class="right-top-btnContainer">
            <el-button type="primary" icon="plus" @click="dialogEdit.isShow=true;dialogEdit.role={}">添加角色</el-button>
        </section>
        <article class="search">
            <section>
                <i>权限</i>
                <SelPermissionTree v-model="fetchParam.permission" :onChange="fetchData"></SelPermissionTree>
            </section>
            <section>
                <!--<el-button type="primary" icon="search" @click="fetchData">搜索</el-button>-->
            </section>
        </article>

        <el-table class="data-table" v-loading="loading"
                  :data="data"
                  :fit="true"
                  border>
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
        </el-table>

        <!--添加和修改的弹出框-->
        <DialogEditRole :visible.sync="dialogEdit.isShow" :role.sync="dialogEdit.role"
                        :onSave="type=>{type === 'add' && fetchData()}"
                        :title="dialogEdit.title"></DialogEditRole>
    </article>
</template>

<script>
    import rolesService from '../../services/roleService'
    import DialogEditRole from './components/DialogEditRole.vue'
    import SelPermissionTree from '../../components/select/PermissionTree.vue'
    export default {
        data() {
            return {
                loading: false,
                data: [],
                fetchParam: {
                    permission: void 0
                },
                dialogEdit: {
                    isShow: false,
                    role: void 0,
                }
            }
        },
        created() {
        },
        activated() {
            this.fetchData()
        },
        methods: {
            async fetchData () {
                this.loadingData = true
                let ret = await rolesService.getRoles(Object.assign({}, this.fetchParam, {permission: this.fetchParam.permission ? this.fetchParam.permission.id : null})).catch(() => {
                    this.loadingData = false
                })
                this.loadingData = false
                this.data = ret.items
            },
            permissionConfirm () {
            }
        },
        components: {DialogEditRole, SelPermissionTree}
    }
</script>
