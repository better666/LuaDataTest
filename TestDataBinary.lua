
local function readInt(file)
    local bytes = file:read(4)
    if not bytes then return nil end
    local value = 0
    for i = 1, 4 do
        value = value + string.byte(bytes, i) * (256 ^ (i-1))
    end
    return value
end

local function readString(file)
    local len = readInt(file)
    if not len then return nil end
    local str = file:read(len)
    return str
end

local data = {}
local meta = {
    __index = function(t,k)
        if k == 'ID' then return t[1]
        elseif k == 'HP' then return t[2]
        elseif k == 'Attack' then return t[3]
        elseif k == 'Defense' then return t[4]
        elseif k == 'Speed' then return t[5]
        elseif k == 'Luck' then return t[6]
        elseif k == 'Magic' then return t[7]
        elseif k == 'Resistance' then return t[8]
        elseif k == 'Agility' then return t[9]
        elseif k == 'Name' then return t[10]
        end
        return nil
    end
}

-- 使用相对于Lua文件的路径
local file = io.open('./TestDataBinary.bin', 'rb')
if not file then 
    print('Failed to open binary file')
    return nil 
end

local count = readInt(file)
if not count then 
    print('Failed to read count')
    file:close()
    return nil
end

for i = 1, count do
    local entry = {
        readInt(file),    -- ID
        readInt(file),    -- HP
        readInt(file),    -- Attack
        readInt(file),    -- Defense
        readInt(file),    -- Speed
        readInt(file),    -- Luck
        readInt(file),    -- Magic
        readInt(file),    -- Resistance
        readInt(file),    -- Agility
        readString(file)  -- Name
    }
    setmetatable(entry, meta)
    data[i] = entry
end

file:close()
return data