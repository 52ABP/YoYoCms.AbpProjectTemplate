<!--登录尝试记录-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/mixins";
    .index-dialog__login-attemps {
        .el-dialog {
            top: 10px !important;
        }
        .content-items {
            height: 150px;
            background: #fafafa;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.18);
            margin-bottom: 15px;
            > * {
                display: inline-block;
            }

            /*左边的头像和登录结果*/
            section {
                width: 50px;
                text-align: center;
                margin: 20px 0 0 40px;
                img {
                    display: block;
                    width: 50px;
                    height: 50px;
                    border-radius: 50%;
                    margin-bottom: 5px;
                }
            }

            /*右边的详情*/
            ul {
                margin-top: 20px;
                vertical-align: top;
                li {
                    min-height: 30px;
                    @extend %singline;
                }
                em {
                    font-weight: bold;
                    width: 5em;
                    display: inline-block;
                    text-align: right;
                    margin-right: 25px;
                }
            }
        }
    }
</style>

<template>
    <el-dialog class="index-dialog__login-attemps"
               :title="L('LoginAttempts')"
               :visible.sync="dialogVisible"
               @open="handleOpen"
               size="tiny">
        <div v-for="item in data" class="content-items">
            <section class="item-left">
                <img :src="user.portrait">
                <el-tag type="success" v-if="item.result === 'Success'">{{L('Success')}}</el-tag>
                <el-tag type="danger" v-else>{{L('Failed')}}</el-tag>
            </section>
            <ul>
                <!--ip地址-->
                <li><em>{{L('IpAddress')}}</em>
                    <i>{{item.clientIpAddress}}</i>
                </li>
                <!--客户端-->
                <li><em>{{L('Client')}}</em>
                    <i>{{item.clientName}}</i>
                </li>
                <!--浏览器-->
                <li><em>{{L('Browser')}}</em>
                    <i>{{item.browserInfo}}</i>
                </li>
                <!--时间-->
                <li><em>{{L('Time')}}</em>
                    <i>{{item.creationTime | date2calendar}}</i>
                </li>
            </ul>
        </div>
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">{{L('Cancel')}}</el-button>
          </span>
    </el-dialog>
</template>

<script>
    import userLoginService from '../../services/administration/userLoginService'
    export default {
        props: {
            visible: Boolean,
        },
        data() {
            return {
                user: this.$store.state.auth.user,
                dialogVisible: false,
                data: [],
                loading: false
            }
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('update:visible', val)
            },
        },
        created() {
        },
        methods: {
            async handleOpen() {
                this.loading = true
                this.data = (await userLoginService.getRecentUserLoginAttempts()).items
                console.log(this.data)
                this.loading = false
            }
        },
        components: {}
    }
</script>
