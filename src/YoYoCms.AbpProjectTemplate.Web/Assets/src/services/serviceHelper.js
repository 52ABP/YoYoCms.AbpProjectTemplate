/**
 * Created by huanghx on 2017/6/30.
 */
export default {
    requireService(module) {
        if (!module) {
            console.trace()
            throw String('模块引入错误, 请输入service模块名')
        }
        return abp.services.yoyocms[module]
    }
}