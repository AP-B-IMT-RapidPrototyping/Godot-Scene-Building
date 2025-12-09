#!/usr/bin/env bash
#
# remove_col_suffix.sh
# Removes the "-col" suffix (before the extension, if present)
# from all files and directories in the current directory.

# Enable recursive search if desired
# Add "-maxdepth 1" to `find` to make it non-recursive

find . -depth -name '*-col*' | while IFS= read -r f; do
    dir=$(dirname "$f")
    base=$(basename "$f")
    
    # Remove "-col" only if it's at the end of the name or before extension
    newbase=$(echo "$base" | sed -E 's/-col(\.[^.]*)?$/\1/')
    
    # Skip if no change
    if [[ "$base" != "$newbase" ]]; then
        newpath="$dir/$newbase"
        echo "Renaming: $f → $newpath"
        mv -n "$f" "$newpath"
    fi
done

echo "✅ All '-col' suffixes removed."
