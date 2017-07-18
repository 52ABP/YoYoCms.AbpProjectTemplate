<!--语言列表-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration__language-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }

        .tip_canntdelete-delete {
            margin-top: 15px;
            color: #333;
        }
    }
</style>

<template>
    <article class="administration__language-container">
        <!--右上角按钮-->
        <section class="right-top-btnContainer">
            <!--创建新语言-->
            <el-button v-if="HasP('Create')" type="primary" icon="plus" class="waves-effect" @click="showAddDialog()">
                {{L('CreateNewLanguage')}}
            </el-button>
        </section>

        <el-table class="data-table" v-loading="loading"
                  :data="data"
                  :fit="true"
                  border>
            <!--<el-table-column type="selection"></el-table-column>-->
            <el-table-column
                    min-width="120"
                    :label="L('Name')">
                <template scope='scope'>
                    <i :class="scope.row.icon"></i> {{scope.row.displayName}}
                    <el-tag type="primary" v-if="defaultLanguageName == scope.row.name">默认</el-tag>
                </template>
            </el-table-column>
            <el-table-column
                    min-width="120"
                    prop="name"
                    :label="L('Code')">
            </el-table-column>
            <el-table-column
                    width="100"
                    :label="L('Default')">
                <template scope="scope">
                    <el-tag type="success" v-if="tenantId != scope.row.tenantId">{{L('Yes')}}</el-tag>
                    <el-tag type="gray" v-else>{{L('No')}}</el-tag>
                </template>
            </el-table-column>
            <el-table-column
                    min-width="100"
                    :label="L('CreationTime')">
                <template scope="scope">
                    <i>{{scope.row.creationTime | date2str}}</i>
                </template>
            </el-table-column>
            <el-table-column
                    width="110"
                    :label="L('Actions')">
                <template scope="scope">
                    <el-dropdown trigger="click">
                        <el-button type="primary" size="small" class="waves-effect">
                            {{L('Actions')}}
                            <i class="el-icon-caret-bottom el-icon--right"></i>
                        </el-button>
                        <el-dropdown-menu slot="dropdown">
                            <el-dropdown-item v-if="HasP('ChangeTexts')">
                                <!--改变文本信息-->
                                <div @click="$router.push({name:'Administration.languagestext', params:{lang: scope.row.name}})">
                                    {{L('ChangeTexts')}}
                                </div>
                            </el-dropdown-item>
                            <el-dropdown-item v-if="defaultLanguageName != scope.row.name">
                                <!--设置当前语言为默认语言-->
                                <div @click="setDefaultLang(scope.row)">
                                    {{L('SetAsDefaultLanguage')}}
                                </div>
                            </el-dropdown-item>
                            <el-dropdown-item v-if="tenantId == scope.row.tenantId && HasP('Edit')">
                                <!--修改-->
                                <div @click="showAddDialog(scope.row)">{{L('Edit')}}</div>
                            </el-dropdown-item>
                            <el-dropdown-item divided v-if="tenantId == scope.row.tenantId && HasP('Delete')">
                                <!--删除-->
                                <div @click="del(scope.$index, scope.row)">{{L('Delete')}}</div>
                            </el-dropdown-item>
                        </el-dropdown-menu>
                    </el-dropdown>
                </template>
            </el-table-column>
        </el-table>
        <!--不能删除的提示-->
        <section class="tip_canntdelete-delete">
            * {{L('CanNotEditOrDeleteDefaultLanguages')}}
        </section>

        <!--创建新语言弹出框-->
        <el-dialog :title="L('CreateNewLanguage')" :visible.sync="dialogAdd.isShow" size="tiny">
            <SelLanguage v-model="dialogAdd.model"></SelLanguage>
            <span slot="footer" class="dialog-footer">
            <el-button @click="dialogAdd.isShow = false">{{L('Cancel')}}</el-button>
            <el-button type="primary" @click="save">{{L('Save')}}</el-button>
          </span>
        </el-dialog>
    </article>
</template>

<script>
    import langService from '../../services/common/languageService'
    import SelLanguage from '../../components/select/Language.vue'
    export default {
        data() {
            return {
                loading: false,
                data: [], // 表格数据
                defaultLanguageName: void 0, // 默认语言
                user: this.$store.state.auth.user || {},
                tenantId: abp.session.tenantId,
                dialogAdd: {
                    isShow: false,
                    model: {}
                }
            }
        },
        activated () {
        },
        methods: {
            async fetchData () {
                try {
                    this.loading = true
                    let ret = await langService.getLanguages()
                    this.defaultLanguageName = ret.defaultLanguageName
                    this.data = ret.items
                } finally {
                    this.loading = false
                    abp.view.setContentLoading(false)
                }
            },
            // 设置默认语言
            async setDefaultLang (item) {
                await langService.setDefaultLanguage({name: item.name})
                this.defaultLanguageName = item.name
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
            },
            // 删除
            del(index, row) {
                abp.message.confirm(lang.L('LanguageDeleteWarningMessage', `【 ${row.displayName} 】`), async (ret) => {
                    if (!ret) return
                    await langService.deleteLanguage({id: row.id})
                    this.data.splice(index, 1)
                    abp.notify.success(lang.L('SuccessfullyDeleted'), lang.L('Success'))
                })
            },
            // 弹出框 保存按钮点击
            async save() {
                await langService.createOrUpdateLanguage({language: this.dialogAdd.model})
                this.dialogAdd.isShow = false
                this.fetchData()
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
            },
            // 显示添加或修改的弹出框
            showAddDialog (item = {}) {
                this.dialogAdd.isShow = true
                this.dialogAdd.model = {}
                this.$nextTick(() => {
                    this.dialogAdd.model = item
                })
            }
        },
        components: {SelLanguage}
    }
</script>
