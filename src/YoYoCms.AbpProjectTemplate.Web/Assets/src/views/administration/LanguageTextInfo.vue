<!--语言列表文本信息-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration__languagetextinfo-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }

        .dialog__edit-langtext {
            section {
                font-size: 14px;
                line-height: 2.2em;
                em {
                    width: 10em;
                    display: block;
                    font-weight: bold;
                }
            }
        }
    }
</style>

<template>
    <article class="administration__languagetextinfo-container">

        <!--搜索-->
        <article class="search">
            <!--搜索-->
            <section>
                <i>{{L('Search')}}</i>
                <el-input v-model="fetchParam.filterText" @keyup.enter.native="fetchData"></el-input>
            </section>
            <!--默认语言-->
            <section>
                <i>{{L('BaseLanguage')}}</i>
                <LangFlag :flag.sync="baseFlag" :displayName.sync="baseDisplayName" :onChange="fetchData"
                          v-model="fetchParam.baseLanguageName"></LangFlag>
            </section>
            <!--目标语言-->
            <section>
                <i>{{L('TargetLanguage')}}</i>
                <LangFlag :flag.sync="targetFlag" :onChange="fetchData" :displayName.sync="targetDisplayName"
                          v-model="fetchParam.targetLanguageName"></LangFlag>
            </section>
            <!--选择源-->
            <section>
                <i>{{L('Source')}}</i>
                <el-select @change="fetchData" v-model="fetchParam.sourceName" :placeholder="L('Select')">
                    <el-option label="AbpProjectTemplate" value="AbpProjectTemplate"></el-option>
                    <el-option label="Abp" value="Abp"></el-option>
                    <el-option label="AbpWeb" value="AbpWeb"></el-option>
                    <el-option label="AbpZero" value="AbpZero"></el-option>
                </el-select>
            </section>
            <!--目标值-->
            <section>
                <i>{{L('TargetValue')}}</i>
                <el-select @change="fetchData" v-model="fetchParam.targetValueFilter" :placeholder="L('Select')">
                    <el-option :label="L('All')" value=""></el-option>
                    <el-option :label="L('EmptyOnes')" value="EMPTY"></el-option>
                </el-select>
            </section>
        </article>

        <el-table class="data-table" v-loading="loading"
                  :data="data"
                  :fit="true"
                  border>
            <el-table-column
                    prop="key"
                    :label="L('Key')">
            </el-table-column>
            <el-table-column
                    prop="baseValue"
                    :label="L('BaseValue')">
            </el-table-column>
            <el-table-column
                    prop="targetValue"
                    :label="L('TargetValue')">
            </el-table-column>
            <el-table-column
                    width="80"
                    :label="L('Edit')">
                <template scope='scope'>
                    <el-button @click="showDialogEdit(true, scope.row)" class="waves-effect"
                               size="mini" icon="edit" :title="L('Edit')"></el-button>
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

        <!--// 修改的弹出框-->
        <el-dialog class="dialog__edit-langtext"
                   :title="L('EditText')"
                   :visible.sync="dialogEdit.isShow"
                   size="tiny">
            <article>
                <!--键值-->
                <section>
                    <em>{{L('Key')}}</em>
                    <i style="font-weight: bold">
                        {{dialogEdit.model.key}}
                    </i>
                </section>
                <section>
                    <em><i :class="baseFlag"></i> {{baseDisplayName}} </em>
                    <i>
                        <el-input type="textarea" :disabled="true" :value="dialogEdit.model.baseValue">
                        </el-input>
                    </i>
                </section>
                <section>
                    <em><i :class="targetFlag"></i> {{targetDisplayName}} </em>
                    <i>
                        <el-input @keyup.enter.native="saveLang" ref="txtDialogTarget" type="textarea"
                                  v-model="dialogEdit.model.targetValue">
                        </el-input>
                    </i>
                </section>
            </article>
            <span slot="footer" class="dialog-footer">
                <el-button @click="showDialogEdit(false)">{{L('Cancel')}}</el-button>
                <el-button type="primary" @click="saveLang">{{L('Save')}}</el-button>
            </span>
        </el-dialog>
    </article>
</template>

<script>
    import langService from '../../services/common/languageService'
    import SelLanguage from '../../components/select/Language.vue'
    import LangFlag from '../../components/select/LanguageFlag.vue'
    import clone from 'clone'
    export default {
        data() {
            return {
                loading: false,
                data: [], // 表格数据
                total: 0,
                baseFlag: void 0, // 默认语言的flag
                baseDisplayName: void 0,
                targetFlag: void 0, // 目标语言的flag
                targetDisplayName: void 0,
                fetchParam: {
                    maxResultCount: 15,
                    skipCount: 0,
                    sorting: void 0,
                    sourceName: 'AbpProjectTemplate',
                    baseLanguageName: 'zh-CN',
                    targetLanguageName: void 0,
                    targetValueFilter: '',
                    filterText: void 0,
                    page: 1
                },
                dialogEdit: {
                    isShow: false,
                    model: {},
                    orign: void 0, // 原始值
                }
            }
        },
        activated () {
            this.fetchParam.targetLanguageName = this.$route.params.lang
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
                    this.loading = true
                    let ret = await langService.getLanguageTexts(this.fetchParam)
                    this.data = ret.items
                    this.total = ret.totalCount
                } finally {
                    this.loading = false
                    abp.view.setContentLoading(false)
                }
            },
            // 显示或隐藏 修改弹出框
            showDialogEdit (isShow, item) {
                this.dialogEdit.isShow = isShow

                if (!isShow) return
                this.dialogEdit.orign = item
                this.dialogEdit.model = clone(item)
                this.$nextTick(() => {
                    this.$refs.txtDialogTarget.$el.querySelector('textarea').focus()
                })
            },
            // 保存语言
            async saveLang() {
                await langService.updateLanguageText({
                    languageName: this.fetchParam.targetLanguageName,
                    sourceName: this.fetchParam.sourceName,
                    value: this.dialogEdit.model.targetValue,
                    key: this.dialogEdit.model.key
                })
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
                Object.assign(this.dialogEdit.orign, this.dialogEdit.model)
                this.dialogEdit.isShow = false
//                this.dialogEdit.model=
            }
        },
        components: {SelLanguage, LangFlag}
    }
</script>
