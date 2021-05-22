using AutoMapper;
using MetricsAgent.Responses;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();
            CreateMap<NetworkMetric, NetworkMetricDto>();
            CreateMap<DotNetMetric, DotNetMetricDto>();
        }
    }
}