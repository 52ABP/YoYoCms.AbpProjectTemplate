<!--消息列表-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .common__notification-container {
        @extend %content-container;

        .search {
            @extend %top-search-container;
        }
    }
</style>

<template>
    <article class="common__notification-container">
        <!--右上角按钮-->
        <section class="right-top-btnContainer">
            <el-button>test</el-button>
        </section>

        <!--搜索-->
        <article class="search">
            <section>
                <i>{{L('Search')}}</i>
                <el-select v-model="fetchParam.state" @change="fetchData">
                    <el-option :label="L('All')" value=""></el-option>
                    <el-option :label="L('Unread')" :value="0"></el-option>
                </el-select>
            </section>
            <section>
                <el-button class="waves-effect" @click="fetchData">{{L('Refresh')}}</el-button>
            </section>
        </article>

        <el-table class="data-table" v-loading="loadingData"
                  :data="data"
                  :fit="true"
                  border>
            <!--<el-table-column type="selection"></el-table-column>-->
            <el-table-column
                    min-width="120"
                    prop="notification.data.message"
                    :label="L('Notifications')">
            </el-table-column>
            <el-table-column
                    width="220"
                    :label="L('CreationTime')">
                <template scope='scope'>
                    {{scope.row.notification.creationTime | date2calendar}}
                </template>
            </el-table-column>
            <el-table-column
                    fixed="right"
                    width="200"
                    :label="L('Actions')">
                <template scope="scope">
                    <el-button class="waves-effect" @click="setReaded(scope.row)" type="primary" size="mini"
                               v-if="scope.row.state == 0">
                        {{L('SetAsRead')}}
                    </el-button>
                    <el-tag type="gray" v-else>{{L('Readed')}}</el-tag>
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
    </article>
</template>

<script>
    import notificationService from '../../services/common/notificationService'
    export default {
        data() {
            return {
                loadingData: false,
                data: [], // 表格数据
                total: 0,
                fetchParam: {
                    state: '',
                    maxResultCount: 15,
                    skipCount: 0,
                    page: 1,
                },
            }
        },
        created() {
            this.fetchData()
        },
        methods: {
            async fetchData() {
                this.loadingData = true
                let ret = await notificationService.getUserNotifications(this.fetchParam)
                this.data = ret.items
                this.total = ret.totalCount

                this.loadingData = false
            },
            handleCurrentChange (val) {
                this.fetchParam.skipCount = Math.abs((val - 1)) * this.fetchParam.maxResultCount
                this.fetchData()
            },
            handleSizeChange (val) {
                this.fetchParam.maxResultCount = val
                this.fetchData()
            },
            async setReaded(item) {
                this.loadingData = true
                await notificationService.setNotificationAsRead({id: item.id})
                this.$store.dispatch('setNotificationReaded', {data: item})
                item.state = 1
                this.loadingData = false
            }
        },
        components: {}
    }
</script>
