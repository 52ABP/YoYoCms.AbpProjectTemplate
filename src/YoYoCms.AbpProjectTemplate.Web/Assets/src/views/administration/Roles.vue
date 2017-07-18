<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-roles-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }
    }
</style>

<template>
    <article class="administration-roles-container">
        <!--右上角按钮-->
        <section class="right-top-btnContainer">
            <el-button class="waves-effect" type="primary" icon="plus" v-if="HasP('Create')"
                       @click="dialogEdit.isShow=true;dialogEdit.role={}">{{L('CreateNewRole')}}
            </el-button>
        </section>
        <!--搜索-->
        <article class="search">
            <section>
                <i>{{L('Permissions')}}</i>
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
                    min-width="120"
                    :label="L('RoleName')">
                <template scope="scope">
                    <i>{{scope.row.displayName}}</i>
                    <el-tooltip :content="L('StaticRole_Tooltip')" placement="top" v-if="scope.row.isStatic">
                        <el-tag type="success">{{L('Static')}}</el-tag>
                    </el-tooltip>

                    <el-tooltip :content="L('DefaultRole_Description')" placement="top" v-if="scope.row.isDefault">
                        <el-tag type="gray">{{L('Default')}}</el-tag>
                    </el-tooltip>
                </template>
            </el-table-column>
            <el-table-column
                    width="190"
                    prop="lastLoginTime"
                    :label="L('CreationTime')">
                <template scope="scope">
                    <i>{{scope.row.creationTime | date2str}}</i>
                </template>
            </el-table-column>
            <el-table-column v-if="HasP('Edit', 'Delete')"
                             width="110"
                             :label="L('Actions')">
                <template scope="scope">
                    <el-dropdown trigger="click">
                        <el-button type="primary" size="small" class="waves-effect">
                            {{L('Actions')}}
                            <i class="el-icon-caret-bottom el-icon--right"></i>
                        </el-button>
                        <!--编辑-->
                        <el-dropdown-menu slot="dropdown">
                            <!--编辑-->
                            <el-dropdown-item v-if="HasP('Edit')">
                                <div @click="dialogEdit.isShow=true;dialogEdit.role=scope.row">
                                    {{L('Edit')}}
                                </div>
                            </el-dropdown-item>
                            <!--删除-->
                            <el-dropdown-item v-if="HasP('Delete') && !scope.row.isStatic">
                                <div @click="del(scope.$index,scope.row)">{{L('Delete')}}</div>
                            </el-dropdown-item>
                        </el-dropdown-menu>
                    </el-dropdown>

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
    import rolesService from '../../services/administration/roleService'
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
        },
        methods: {
            async fetchData () {
                try {
                    this.loadingData = true
                    let ret = await rolesService.getRoles(Object.assign({}, this.fetchParam, {permission: this.fetchParam.permission ? this.fetchParam.permission.id : null}))
                    this.data = ret.items
                } finally {
                    this.loadingData = false
                    abp.view.setContentLoading(false)
                }
            },
            // 删除
            del(index, item) {
                abp.message.confirm(lang.L('RoleDeleteWarningMessage', item.displayName), async (ret) => {
                    if (!ret) return
                    await rolesService.deleteRole({id: item.id})
                    this.data.splice(index, 1)
                    abp.notify.success(lang.L('SuccessfullyDeleted'), lang.L('Success'))
                })
            },
            permissionConfirm () {
            }
        },
        components: {DialogEditRole, SelPermissionTree}
    }
</script>
