/**
 * Created by huanghx on 2017/7/12.
 */
// 权限插件
export default {
    install: function (Vue, options) {
        // 3. 注入组件
        Vue.mixin({
            methods: {
                HasP(...args) {
                    let ret = []
                    if (this.$route.meta && this.$route.meta.permission) {
                        for (let i = 0; i < args.length; i++) {
                            let item = args[i]
                            if (item.indexOf(this.$route.meta.permission) < 0)
                                item = this.$route.meta.permission + '.' + item
                            ret.push(item)
                        }
                    }

                    return abp.auth.isGranted(...ret)
                }
            }
        })
    }
}