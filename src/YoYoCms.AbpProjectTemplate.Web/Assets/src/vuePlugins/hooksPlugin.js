/**
 * Created by huanghx on 2017/7/5.
 */

import config from '../common/config'
export default {
    install: function (Vue, options) {
        // 3. 注入组件
        Vue.mixin({
            mounted () {
                Waves.init()
            },
            activated() {
                // 如果不显示tab 则直接请求数据
                if (!config.showPageTab) {
                    setTimeout(() => {
                        this.fetchData && this.fetchData()
                    }, 0)
                    return
                }
                let isExistTab = false
                // 判断当前页面是否在tab中
                for (let i = 0; i < this.$store.state.index.pageTab.length; i++) {
                    let item = this.$store.state.index.pageTab[i]
                    if (item.name === this.$route.name) isExistTab = true
                }
                setTimeout(() => {
                    // 如果没有在tab中 则请求数据
                    if (!isExistTab && this.fetchData) this.fetchData()
                    // 如果在tab中 则 关掉loading
                    else
                        abp.view.setContentLoading(false)
                }, 0)
            }
        })
    }
}