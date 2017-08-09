<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-maintenance-container {
        @extend %content-container;

        /*position: absolute;*/
        /*height: 90%;*/
        /*overflow-y: auto;*/

        .search {
            @extend %top-search-container;
        }

        %commonHeader {
            h2 {
                font-size: 18px;
                .clearall--btn {
                    text-align: right;
                }
            }
        }

        .first-cache {
            @extend %commonHeader;
        }

        .second-log {
            @extend %commonHeader;

            .second-log-content {
                height: 80%;
                overflow-y: auto;
                position: relative;

                section {
                    line-height: 2.3em;
                }
            }
        }
    }
</style>

<template>
    <article class="administration-maintenance-container" v-loading="loading">
        <el-tabs v-model="activeName" @tab-click="handleTabClick">
            <el-tab-pane :label="L('Caches')" name="first" class="first-cache">
                <h2 class="row">
                    <em class="col-xs-6">[{{L('CachesHeaderInfo')}}]</em>
                    <div class="clearall--btn col-xs-6">
                        <el-button size="tiny" type="primary" @click="clearCache(null, true)">
                            <i class="material-icons material-icons-small">refresh</i>
                            {{L('ClearAll')}}
                        </el-button>
                    </div>
                </h2>

                <el-table
                        :fit="true"
                        stripe
                        :show-header="false"
                        v-loading="loading"
                        :data="caches.data">
                    <el-table-column prop="name" min-width="200">
                    </el-table-column>
                    <el-table-column width="105">
                        <template scope="scope">
                            <el-button type="danger" size="small" @click="clearCache(scope.row)">{{L('Clear')}}
                            </el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </el-tab-pane>
            <el-tab-pane :label="L('WebSiteLogs')" name="second" class="second-log">
                <h2 class="row">
                    <em class="col-xs-6">[{{L('WebSiteLogsHeaderInfo')}}]</em>
                    <div class="clearall--btn col-xs-6">
                        <el-button size="tiny" type="primary" @click="exportWeblog">
                            <i class="material-icons material-icons-small">file_download </i>
                            {{L('DownloadAll')}}
                        </el-button>
                        <el-button size="tiny" type="primary" @click="getWeblog">
                            <i class="material-icons material-icons-small">refresh</i>
                            {{L('Refresh')}}
                        </el-button>
                    </div>
                </h2>
                <article class="second-log-content" :style="{height:weblogs.contentHeight + 'px'}">
                    <section v-for="item in weblogs.data">
                        <el-tag type="primary" v-if="item.indexOf('DEBUG') == 0">DEBUG</el-tag>
                        <el-tag type="warning" v-else-if="item.indexOf('WARN') == 0">WARN</el-tag>
                        <el-tag type="danger" v-else-if="item.indexOf('ERROR') == 0">ERROR</el-tag>
                        {{item}}
                    </section>
                </article>
            </el-tab-pane>
        </el-tabs>
    </article>
</template>

<script>
    import cachingService from '../../services/administration/cachingService'
    import weblogService from '../../services/administration/weblogService'
    export default {
        data() {
            return {
                activeName: 'first',
                loading: false,
                caches: {
                    data: [],
                },
                weblogs: {
                    data: [],
                    contentHeight: 0
                }
            }
        },
        created() {
        },
        activated() {
        },
        mounted() {
            this.weblogs.contentHeight = window.innerHeight - 400
        },
        methods: {
            handleTabClick(item) {
                if (item.name === 'second' && this.weblogs.data.length < 1) {
                    this.getWeblog()
                }
            },
            async fetchData() {
                this.loading = true
                this.caches.data = (await cachingService.getAllCaches()).items

                this.loading = false
                abp.view.setContentLoading(false)
            },
            // isAll 是否清除所有
            async clearCache (item, isAll) {
                try {
                    this.loading = true
                    if (isAll) await cachingService.clearAllCaches()
                    else await cachingService.clearCache({id: item.name})
                    abp.notify.success(lang.L('CacheSuccessfullyCleared'), lang.L('Success'))
                } finally {
                    this.loading = false
                }
            },
            async getWeblog() {
                try {
                    this.loading = true
                    this.weblogs.data = (await weblogService.getLatestWebLogs()).latesWebLogLines
                } finally {
                    this.loading = false
                }
            },
            exportWeblog() {
                weblogService.exportAll()
            }
        },
        components: {}
    }
</script>
