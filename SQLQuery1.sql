Select r.Name, Count(v.Id) as "versions" from Repository r, version v
where r.Id = v.RepositoryId
group by r.Name